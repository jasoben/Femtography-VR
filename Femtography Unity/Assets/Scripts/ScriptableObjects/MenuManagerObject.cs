using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "menuObject", menuName = "Menu Manager/Menu Object", order = 1)]
public class MenuManagerObject : ScriptableObject
{
    public bool isActive, isFlashing, isOn;

    private void OnEnable()
    {
        isActive = false;
        isFlashing = false;
    }

    public void SetOnOrOff(bool isNowOnOrOff)
    {
        isOn = isNowOnOrOff;
    }

    public void SetActive(bool isNowActive)
    {
        isActive = isNowActive;
    }

    public void SetFlashing(bool isNowFlashing)
    {
        isFlashing = isNowFlashing;
    }

}
