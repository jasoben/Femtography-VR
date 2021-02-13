using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleHelper : MonoBehaviour
{
    public UnityEvent boolOn, boolOff;
    public GlobalBool globalBool;
    PhysicalToggle physicalToggle;
    // Start is called before the first frame update
    void Start()
    {
        physicalToggle = GetComponent<PhysicalToggle>();
    }

    // Update is called once per frame
    void Update()
    {
        if (physicalToggle.IsToggled != globalBool.boolValue)
            physicalToggle.SetToggle(globalBool.boolValue);
    }


    public void ToggleOnOrOff()
    {
        globalBool.boolValue = !globalBool.boolValue;
        physicalToggle.SetToggle(globalBool.boolValue);
        if (globalBool.boolValue)
            boolOn.Invoke();
        else
            boolOff.Invoke();
    }
}
