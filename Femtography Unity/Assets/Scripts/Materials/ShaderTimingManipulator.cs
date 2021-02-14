using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This advances the time for shaders with time variables. The reason we don't use the built-in time node
// for the Shader Graph is that when we change the game speed it causes the numbers to fluxuate wildly from 
// current time (e.g. 13213) to zero, causing lots of strange visual behaviors based on 
public class ShaderTimingManipulator : MonoBehaviour
{
    public VariableSlider gameSpeed;
    float currentTime = 0;

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime * gameSpeed.value;
        GetComponent<Renderer>().material.SetFloat("Time_", currentTime);
    }

    public float GetTime()
    {
        return currentTime;
    }

}
