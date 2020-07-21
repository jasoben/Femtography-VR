using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class ThePlayerController : MonoBehaviour
{
    public Particle particle;
    public GlobalBool VRorNot;
    public Toggle followToggle;

    // Start is called before the first frame update
    void Start()
    {
        if (XRDevice.isPresent)
            VRorNot.boolValue = true;
        else
            VRorNot.boolValue = false;
    }

    public void DetermineMoveOrNot()
    {
        if (followToggle.isOn)
            BroadcastMessage("StartMoving");
    }
}
