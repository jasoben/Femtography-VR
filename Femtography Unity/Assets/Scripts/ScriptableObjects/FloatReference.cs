using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FloatReference 
{
    public bool UseConstant = true;
    [Range(0,1)]
    public float ConstantValue;
    public VariableSlider variableSlider;

    public float Value
    {
        get
        {
            return UseConstant ? ConstantValue : variableSlider.value;
        }

        set 
        {
            ConstantValue = value;
            variableSlider.value = value;
        }
    }
}
