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
        GetComponent<CircleCollider2D>().radius = Constants.WolfColliderRadiusBig;
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
        //play the sound
        GetComponent<AudioSource>().Play();
        //show the trail renderer
        GetComponent<TrailRenderer>().enabled = true;
        //allow for gravity forces
        GetComponent<Rigidbody2D>().isKinematic = false;
        //make the collider normal size
        GetComponent<CircleCollider2D>().radius = Constants.WolfColliderRadiusNormal;
        State = WolfState.Shot;
    }

    IEnumerator DestroyAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

    public WolfState State
    {
        get;
        private set;
    }
}
