using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuManager : MonoBehaviour
{
    public GlobalBool isVehicleInFinalPosition, isFirstElectron, isFollowingParticles, 
        mouseLookBool, electronInTransit, isFirstPlayThrough;
    public UnityEvent setFollow, setUnFollow, mouseLookOn, mouseLookOff, setKeyEvents, setNoKeyEvents, textYes, textNo;
    public MenuManagerObject followToggle, keyEventsToggle, textToggle;

    private void Start()
    {
        followToggle.isOn = false;
        keyEventsToggle.isOn = true;
        textToggle.isOn = true;
        electronInTransit.boolValue = false;
    }

    public void EnableFollowToggle()
    {
        //if (!isVehicleInFinalPosition.boolValue)
            followToggle.SetActive(true);
    }

    public void HighlightFollowToggle()
    {
        if (!isFirstElectron.boolValue && !isVehicleInFinalPosition.boolValue)
            followToggle.SetFlashing(true);
    }

    private void Update()
    {
        FlashingController.Flashing(); // Change the global flashing variable
        if (Input.GetKeyDown(KeyCode.F) && !electronInTransit.boolValue)
        {
            if (isFollowingParticles.boolValue)
                setUnFollow.Invoke();
            else if (!isFollowingParticles.boolValue)
                setFollow.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (keyEventsToggle.isOn && !isFirstPlayThrough.boolValue)
            {
                setNoKeyEvents.Invoke();
            }
            else if (!keyEventsToggle.isOn && !isFirstPlayThrough.boolValue)
            {
                setKeyEvents.Invoke();
            }
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (textToggle.isOn)
            {
                textNo.Invoke();
            }
            else if (!textToggle.isOn)
            {
                textYes.Invoke();
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (mouseLookBool.boolValue)
            {
                mouseLookOff.Invoke();
            }
            else if (!mouseLookBool.boolValue)
            {
                mouseLookOn.Invoke();
            }
        }
    }
}
