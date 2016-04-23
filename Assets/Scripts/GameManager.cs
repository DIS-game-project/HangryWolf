using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts;
using System.Linq;

public class GameManager : MonoBehaviour
{

    public CameraFollow cameraFollow;
    int currentWolfIndex;
    public Cannon cannon;
    [HideInInspector]
    public static GameState CurrentGameState = GameState.Start;
    private List<GameObject> Bricks;
    private List<GameObject> Wolves;
    private List<GameObject> Pigs;

    // Use this for initialization
    void Start()
    {
        CurrentGameState = GameState.Start;
        cannon.enabled = false;
        //find all relevant game objects
        Bricks = new List<GameObject>(GameObject.FindGameObjectsWithTag("Brick"));
        Wolves = new List<GameObject>(GameObject.FindGameObjectsWithTag("Wolf"));
        Pigs = new List<GameObject>(GameObject.FindGameObjectsWithTag("Pig"));
        //unsubscribe and resubscribe from the event
        //this ensures that we subscribe only once
        cannon.WolfThrown -= Cannon_WolfThrown;
        cannon.WolfThrown += Cannon_WolfThrown;
    }


    // Update is called once per frame
    void Update()
    {
        switch (CurrentGameState)
        {
            case GameState.Start:
                //if player taps, begin animating the bird 
                //to the cannon
                if (Input.GetMouseButtonUp(0))
                {
                    AnimateWolfToCannon();
                }
                break;
            case GameState.WolfMovingToCannon:
                //do nothing
                break;
            case GameState.Playing:
                //if we have thrown a bird
                //and either everything has stopped moving
                //or there has been 5 seconds since we threw the bird
                //animate the camera to the start position
                if (cannon.cannonState == CannonState.WolfFlying &&
                    (BricksWolvesPigsStoppedMoving() || Time.time - cannon.TimeSinceThrown > 5f))
                {
                    cannon.enabled = false;
                    AnimateCameraToStartPosition();
                    CurrentGameState = GameState.WolfMovingToCannon;
                }
                break;
            //if we have won or lost, we will restart the level
            //in a normal game, we would show the "Won" screen 
            //and on tap the user would go to the next level
            case GameState.Won:
            case GameState.Lost:
                if (Input.GetMouseButtonUp(0))
                {
                    Application.LoadLevel(Application.loadedLevel);
                }
                break;
            default:
                break;
        }
    }


    /// <summary>
    /// A check whether all Pigs are null
    /// i.e. they have been destroyed
    /// </summary>
    /// <returns></returns>
    private bool AllPigsDestroyed()
    {
        return Pigs.All(x => x == null);
    }

    /// <summary>
    /// Animates the camera to the original location
    /// When it finishes, it checks if we have lost, won or we have other birds
    /// available to throw
    /// </summary>
    private void AnimateCameraToStartPosition()
    {
        float duration = Vector2.Distance(Camera.main.transform.position, cameraFollow.StartingPosition) / 10f;
        if (duration == 0.0f) duration = 0.1f;
        //animate the camera to start
        Camera.main.transform.positionTo
            (duration,
            cameraFollow.StartingPosition). //end position
            setOnCompleteHandler((x) =>
                        {
                            cameraFollow.IsFollowing = false;
                            if (AllPigsDestroyed())
                            {
                                CurrentGameState = GameState.Won;
                            }
                            //animate the next bird, if available
                            else if (currentWolfIndex == Wolves.Count - 1)
                            {
                                //no more birds, go to finished
                                CurrentGameState = GameState.Lost;
                            }
                            else
                            {
                                cannon.cannonState = CannonState.Idle;
                                //bird to throw is the next on the list
                                currentWolfIndex++;
                                AnimateWolfToCannon();
                            }
                        });
    }

    /// <summary>
    /// Animates the bird from the waiting position to the cannon
    /// </summary>
    void AnimateWolfToCannon()
    {
        CurrentGameState = GameState.WolfMovingToCannon;
        Wolves[currentWolfIndex].transform.positionTo
            (Vector2.Distance(Wolves[currentWolfIndex].transform.position / 10,
            cannon.WolfWaitPosition.transform.position) / 10, //duration
            cannon.WolfWaitPosition.transform.position). //final position
                setOnCompleteHandler((x) =>
                        {
                            x.complete();
                            x.destroy(); //destroy the animation
                            CurrentGameState = GameState.Playing;
                            cannon.enabled = true; //enable cannon
                            //current wolf is the current in the list
                            cannon.WolfToThrow = Wolves[currentWolfIndex];
                        });

    }

    /// <summary>
    /// Event handler, when the bird is thrown, camera starts following it
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Cannon_WolfThrown(object sender, System.EventArgs e)
    {
        cameraFollow.WolfToFollow = Wolves[currentWolfIndex].transform;
        print("good");
        cameraFollow.IsFollowing = true;
    }

    /// <summary>
    /// Check if all birds, pigs and bricks have stopped moving
    /// </summary>
    /// <returns></returns>
    bool BricksWolvesPigsStoppedMoving()
    {
        foreach (var item in Bricks.Union(Wolves).Union(Pigs))
        {
            if (item != null && item.GetComponent<Rigidbody2D>().velocity.sqrMagnitude > Constants.MinVelocity)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Found here
    /// http://www.bensilvis.com/?p=500
    /// </summary>
    /// <param name="screenWidth"></param>
    /// <param name="screenHeight"></param>
    public static void AutoResize(int screenWidth, int screenHeight)
    {
        Vector2 resizeRatio = new Vector2((float)Screen.width / screenWidth, (float)Screen.height / screenHeight);
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(resizeRatio.x, resizeRatio.y, 1.0f));
    }

    /// <summary>
    /// Shows relevant GUI depending on the current game state
    /// </summary>
    void OnGUI()
    {
        AutoResize(800, 480);
        switch (CurrentGameState)
        {
            case GameState.Start:
                GUI.Label(new Rect(0, 150, 200, 100), "Tap the screen to start");
                break;
            case GameState.Won:
                GUI.Label(new Rect(0, 150, 200, 100), "You won! Tap the screen to restart");
                break;
            case GameState.Lost:
                GUI.Label(new Rect(0, 150, 200, 100), "You lost! Tap the screen to restart");
                break;
            default:
                break;
        }
    }


}
