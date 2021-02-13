using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Change a value smoothly between two values
public class UTIL_SmoothChanger : MonoBehaviour
{
    float endValue, changeSpeed, currentValue, startValue;
    public UnityEvent valueChanged;
    public VariableSlider variableToChange;
    public float Value { get { return currentValue; } }

    public void StartSmoothChange(float startValueP, float changeSpeedP, float endValueP)
    {
        startValue = startValueP;
        endValue = endValueP;
        changeSpeed = changeSpeedP;

        IEnumerator SmoothChanger = SmoothChange();
        StartCoroutine(SmoothChanger);
    }
    IEnumerator SmoothChange()
    {
        float lerpAmount = 0;
        while (lerpAmount < 1)
        {
            currentValue = Mathf.Lerp(startValue, endValue, lerpAmount);
            if (variableToChange != null)
                variableToChange.value = currentValue;
            lerpAmount += changeSpeed;
            yield return new WaitForEndOfFrame();
        }
        currentValue = endValue;
        if (variableToChange != null)
            variableToChange.value = endValue;
        valueChanged.Invoke();
        yield break;
    }

}
