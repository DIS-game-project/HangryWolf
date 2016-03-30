using UnityEngine;
using System.Collections;

public class Destroyer : MonoBehaviour {

        //destroyers are located in the borders of the screen
        //when something hits, they destroy it

    void OnTriggerEnter2D(Collider2D col)
    {
        string tag = col.gameObject.tag;
        if(tag == "Wolf" || tag == "Pig" || tag == "Brick")
        {
            Destroy(col.gameObject);
        }
    }
}