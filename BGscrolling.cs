using UnityEngine;
using System.Collections;

public class Done_BGScroller : MonoBehaviour
{
    public float scrollSpeed;
    //publicly assigned speed of the sky
    public float tileSizeZ;
    //size of the BG tile
    //check the axis of the scrolling!!!

    private Vector3 startPosition;
    //start position of the tile

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeZ);
        //once the tile is scrolled through, it will repeat the tile effectively placing new one right after the old one
        transform.position = startPosition + Vector3.forward * newPosition;
        //no idea what this does. consult teacher.
    }
}