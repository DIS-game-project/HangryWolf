using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System;

public class Cannon : MonoBehaviour
{

    //a vector that points in the middle between left and right parts of the slingshot
    private Vector3 CannonVector;
    [HideInInspector]
    public CannonState cannonState;
    public GameObject Barrel;
    //this linerenderer will draw the projected trajectory of the thrown bird
    public LineRenderer TrajectoryLineRenderer;

    [HideInInspector]
    //the bird to throw
    public GameObject WolfToThrow;
   
    //the position of the bird tied to the slingshot
    public Transform WolfWaitPosition;

    public float ThrowSpeed;

    [HideInInspector]
    public float TimeSinceThrown;

    // Use this for initialization
    void Start()
    {
        //set the sorting layer name for the line renderers
        //for the slingshot renderers this did not work so I
        //set the z on the background sprites to 10
        //hope there's a better way around that!
     
        TrajectoryLineRenderer.sortingLayerName = "Foreground";

        cannonState = CannonState.Idle;
      
        
    }

    // Update is called once per frame
    void Update()
    {

       // print(WolfToThrow.transform.position);
        switch (cannonState)
        {
            case CannonState.Idle:
                //fix bird's position
                InitializeWolf();
               
                
                if (Input.GetMouseButtonDown(0))
                {
                    //get the point on screen user has tapped
                    Vector3 location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    //if user has tapped onto the bird
                    if (WolfToThrow.GetComponent<PolygonCollider2D>() == Physics2D.OverlapPoint(location))
                    {
                        cannonState = CannonState.UserAiming;
                    }
                }
                break;
            case CannonState.UserAiming:

                WolfToThrow.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0f);

                if (Input.GetMouseButton(0))
                {
                    //get where user is tapping
                    Vector3 location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    location.z = 0;
                    //we will let the user pull the bird up to a maximum distance
                    if (Vector3.Distance(location, CannonVector) > 1.5f)
                    {
                        //basic vector maths :)
                        var maxPosition = (location - CannonVector).normalized * 1.5f + CannonVector;
                        WolfToThrow.transform.position = maxPosition;
                    }
                    else
                    {
                        WolfToThrow.transform.position = location;
                    }
                    float distance = Vector3.Distance(CannonVector, WolfToThrow.transform.position);
                    //display projected trajectory based on the distance
                    DisplayTrajectoryLineRenderer2(distance);
                    float xDiff = Camera.main.transform.position.x - WolfToThrow.transform.position.x;
                    float yDiff = Camera.main.transform.position.y - WolfToThrow.transform.position.y;
                    var angle = Math.Atan2(yDiff, xDiff) * (180 / Math.PI);
                    float angle2 = (float)angle;
                    //var angle = Vector3.Angle( WolfWaitPosition.transform.position, Input.mousePosition);
                    print(angle);
                    transform.eulerAngles = new Vector3 (0, 0, angle2);
                  
                }
                else//user has removed the tap 
                {
                    SetTrajectoryLineRenderesActive(false);
                    //throw the bird!!!
                    TimeSinceThrown = Time.time;
                    float distance = Vector3.Distance(CannonVector, WolfToThrow.transform.position);
                    if (distance > 1)
                    {
                       
                        cannonState = CannonState.WolfFlying;
                        ThrowWolf(distance);
                    }
                    else//not pulled long enough, so reinitiate it
                    {
                        //distance/10 was found with trial and error :)
                        //animate the bird to the wait position
                        WolfToThrow.transform.positionTo(distance / 10, //duration
                            WolfWaitPosition.transform.position). //final position
                            setOnCompleteHandler((x) =>
                        {
                            x.complete();
                            x.destroy();
                            InitializeWolf();
                        });

                    }
                }
                break;
            case CannonState.WolfFlying:
                break;
            default:
                break;
        }

    }

    private void ThrowWolf(float distance)
    {
        //get velocity
       Vector3 velocity = CannonVector - WolfToThrow.transform.position;
       WolfToThrow.GetComponent<Wolf>().OnThrow(); //make the bird aware of it
        //old and alternative way
        //BirdToThrow.GetComponent<Rigidbody2D>().AddForce
        //    (new Vector2(v2.x, v2.y) * ThrowSpeed * distance * 300 * Time.deltaTime);
        //set the velocity
        WolfToThrow.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, velocity.y) * ThrowSpeed * distance;
        WolfToThrow.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);

        //notify interested parties that the bird was thrown
        if (WolfThrown != null)
            WolfThrown(this, EventArgs.Empty);
    }

    public event EventHandler WolfThrown;

    private void InitializeWolf()
    {
        //initialization of the ready to be thrown bird
        WolfToThrow.transform.position = WolfWaitPosition.position;
        cannonState = CannonState.Idle;
        
    }

   

    void SetTrajectoryLineRenderesActive(bool active)
    {
        TrajectoryLineRenderer.enabled = active;
    }


    /// <summary>
    /// Another solution (a great one) can be found here
    /// http://wiki.unity3d.com/index.php?title=Trajectory_Simulation
    /// </summary>
    /// <param name="distance"></param>
    void DisplayTrajectoryLineRenderer2(float distance)
    {
        SetTrajectoryLineRenderesActive(true);
        Vector3 v2 = CannonVector - WolfToThrow.transform.position;
        int segmentCount = 15;
        float segmentScale = 2;
        Vector2[] segments = new Vector2[segmentCount];

        // The first line point is wherever the player's cannon, etc is
        segments[0] = WolfToThrow.transform.position;

        // The initial velocity
        Vector2 segVelocity = new Vector2(v2.x, v2.y) * ThrowSpeed * distance;

        float angle = Vector2.Angle(segVelocity, new Vector2(1, 0));
        float time = segmentScale / segVelocity.magnitude;
        for (int i = 1; i < segmentCount; i++)
        {
            //x axis: spaceX = initialSpaceX + velocityX * time
            //y axis: spaceY = initialSpaceY + velocityY * time + 1/2 * accelerationY * time ^ 2
            //both (vector) space = initialSpace + velocity * time + 1/2 * acceleration * time ^ 2
            float time2 = i * Time.fixedDeltaTime * 5;
            segments[i] = segments[0] + segVelocity * time2 + 0.5f * Physics2D.gravity * Mathf.Pow(time2, 2);
        }

        TrajectoryLineRenderer.SetVertexCount(segmentCount);
        for (int i = 0; i < segmentCount; i++)
            TrajectoryLineRenderer.SetPosition(i, segments[i]);
    }



    ///http://opengameart.org/content/forest-themed-sprites
    ///forest sprites found on opengameart.com
    ///© 2012-2013 Julien Jorge <julien.jorge@stuff-o-matic.com>

}
