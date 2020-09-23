using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuManager : MonoBehaviour
{
    public GlobalBool isVehicleInFinalPosition, isFirstElectron, isFollowingParticles, mouseLookBool;
    public UnityEvent setFollow, setUnFollow, mouseLookOn, mouseLookOff;
    public MenuManagerObject followToggle;
    
    public void EnableFollowToggle()
    {
        if (!isVehicleInFinalPosition.boolValue)
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
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isFollowingParticles.boolValue)
                setUnFollow.Invoke();
            else if (!isFollowingParticles.boolValue)
                setFollow.Invoke();
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
