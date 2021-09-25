using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR;

public class ThePlayerController : MonoBehaviour
{
    public Particle particle;
    public GlobalBool VRorNot, followParticles, vehicleIsInFinalPosition;
    public UnityEvent disableFollowToggle;

    // Start is called before the first frame update
    void Start()
    {
        vehicleIsInFinalPosition.boolValue = false;
        //followElectron.boolValue = false;
    }

    public void DetermineMoveOrNot()
    {
        if (followParticles.boolValue && !vehicleIsInFinalPosition.boolValue)
        {
            BroadcastMessage("StartMoving"); // talk to the Transform Object component
            disableFollowToggle.Invoke();
        }
    }

    public void DetermineMoveableOrNot()
    {
        if (followParticles.boolValue && vehicleIsInFinalPosition.boolValue)
        {
            BroadcastMessage("NoLongerMoveable");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "VehiclePositionTrigger")
        {
            vehicleIsInFinalPosition.boolValue = true;
        }
    }
}
