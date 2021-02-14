using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackWithObject : MonoBehaviour
{
    public GameObject objectToTrack;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        gameObject.transform.position = objectToTrack.transform.position;
        gameObject.transform.rotation = objectToTrack.transform.rotation;
    }
}
