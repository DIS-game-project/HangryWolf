using UnityEngine;
using System.Collections;
using Assets.Scripts;

[RequireComponent(typeof(Rigidbody2D))]
public class Wolf : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        //trailrenderer is not visible until we throw the bird
        GetComponent<TrailRenderer>().enabled = false;
        GetComponent<TrailRenderer>().sortingLayerName = "Foreground";
        //no gravity at first
        GetComponent<Rigidbody2D>().isKinematic = true;
        //make the collider bigger to allow for easy touching
       // GetComponent<CircleCollider2D>().radius = Constants.WolfColliderRadiusBig;
        State = WolfState.BeforeShot;
    }



    void FixedUpdate()
    {
        //if we've thrown the bird
        //and its speed is very small
        if (State == WolfState.Shot &&
            GetComponent<Rigidbody2D>().velocity.sqrMagnitude <= Constants.MinVelocity)
        {
            //destroy the bird after 2 seconds
            StartCoroutine(DestroyAfter(2));
        }
    }

    public void OnThrow()
    {
        
		//Play sound when it has been turned on
		if(MainCode.Sound == 1){
			GetComponent<AudioSource>().Play();
		}
        //show the trail renderer
        GetComponent<TrailRenderer>().enabled = true;
        //allow for gravity forces
        GetComponent<Rigidbody2D>().isKinematic = false;
        //make the collider normal size
       // GetComponent<PolygonCollider2D>().radius = Constants.WolfColliderRadiusNormal;
        State = WolfState.Shot;
    }

    IEnumerator DestroyAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.GetComponent<Rigidbody2D>() == null) return;

        float damage = col.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 10;
        //don't play audio for small damages
       
        //decrease health according to magnitude of the object that hit us
        Health -= damage;
        //if health is 0, destroy the block
        if (Health <= 0)
        {

            Destroy(gameObject);
            Instantiate(Corpse, transform.position, transform.rotation);
            //This will create a copy of the remains at the exact position of the brick. Afterwards, the object will be removed, giving the illusion that the new one is actually the old one, but "broken".
        }

    }
    public GameObject Corpse;
    public float Health = 700f;

    public WolfState State
    {
        get;
        private set;
    }
}
