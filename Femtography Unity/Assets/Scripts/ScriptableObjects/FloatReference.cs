using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FloatReference 
{
    public bool UseConstant = true;
    [Range(0,1)]
    public float ConstantValue;
    public VariableSlider variable;

    public float Value
    {
        get
        {
            return UseConstant ? ConstantValue : variable.value;
        }
    }
}
