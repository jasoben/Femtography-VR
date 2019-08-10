using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ThePlayerController : MonoBehaviour
{
    public Particle particle;
    public GlobalBool VRorNot;

    // Start is called before the first frame update
    void Start()
    {
        particle.speed = 0; 
        particle.normalSpeed = 1;
        if (XRDevice.isPresent)
            VRorNot.boolValue = true;
        else
            VRorNot.boolValue = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
