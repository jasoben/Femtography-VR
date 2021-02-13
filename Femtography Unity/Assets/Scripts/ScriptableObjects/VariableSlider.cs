using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class VariableSlider : ScriptableObject
{
    [Range(0,1)]
    public float value;

    public void SetValue(float thisValue)
    {
        value = thisValue;
    }

}
