using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleHelper : MonoBehaviour
{
    public UnityEvent boolOn, boolOff;
    public GlobalBool globalBool;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ToggleOnOrOff()
    {
        globalBool.boolValue = !globalBool.boolValue;
        GetComponent<PhysicalToggle>().SetToggle(globalBool.boolValue);
        if (globalBool.boolValue)
            boolOn.Invoke();
        else
            boolOff.Invoke();
    }
}
