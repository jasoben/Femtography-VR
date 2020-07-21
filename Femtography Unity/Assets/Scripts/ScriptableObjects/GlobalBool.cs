using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GlobalBool : ScriptableObject
{
    public bool boolValue = true;

    public void SetBool(bool newValue)
    {
        boolValue = newValue;
    }
}
