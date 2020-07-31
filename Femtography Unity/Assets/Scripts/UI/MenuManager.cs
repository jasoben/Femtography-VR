using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GlobalBool isVehicleInFinalPosition, isFirstElectron;
    public MenuManagerObject followToggle;
    
    public void EnableFollowToggle()
    {
        if (!isVehicleInFinalPosition.boolValue)
            followToggle.SetActive(true);
    }

    public void HighlightFollowToggle()
    {
        if (!isFirstElectron.boolValue && !isVehicleInFinalPosition)
            followToggle.SetFlashing(true);
    }

    private void Update()
    {
        FlashingController.Flashing(); // Change the global flashing variable
    }
}
