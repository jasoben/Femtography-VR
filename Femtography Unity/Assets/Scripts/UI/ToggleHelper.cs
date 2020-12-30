using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleHelper : MonoBehaviour
{
    public UnityEvent mouseLookOn, mouseLookOff;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ChangeMouseLookStatus()
    {
        bool isOn = GetComponent<PhysicalButton>().IsToggled;
        if (isOn)
            mouseLookOn.Invoke();
        else
            mouseLookOff.Invoke();
    }
}
