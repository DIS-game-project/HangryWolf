using UnityEngine;
using System.Collections;
using Assets.Scripts;

[RequireComponent(typeof(Rigidbody2D))]
public class Wolf : MonoBehaviour
{

    // Initialization
    void Start()
    {
        //trail does not appear before the launch
        GetComponent<TrailRenderer>().enabled = false;
        GetComponent<TrailRenderer>().sortingLayerName = "Foreground"; //in foreground to avoid contact with scenery
       
        //maybe resize collider for easier launch?
      
        State = WolfState.BeforeThrown;
    }





    void FixedUpdate()
    {
        //if the wolf has been launched and is slowly rolling
        if (State == WolfState.Thrown &&
            GetComponent<Rigidbody2D>().velocity.sqrMagnitude <= Constants.MinVelocity)
        {
            //removes wolf from the game after 5 seconds
            StartCoroutine(DestroyAfter(5));
        }
    }

    public void OnThrow()
    {
        //Add sound here
       
        //Spawn trail
        GetComponent<TrailRenderer>().enabled = true;
        //Gravity for projectile
        GetComponent<Rigidbody2D>().isKinematic = false;
        State = WolfState.Thrown;
    }
		


	//possible code for dismemberment
	public void breakWolfJoint()

	{

		http://docs.unity3d.com/ScriptReference/Joint-breakForce.html 


	}


    public WolfState State
    {
        get;
        private set;
    }
}
