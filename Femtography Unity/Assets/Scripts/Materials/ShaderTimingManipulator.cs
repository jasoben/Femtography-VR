using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This advances the time for shaders with time variables. The reason we don't use the built-in time node
// for the Shader Graph is that when we change the game speed it causes the numbers to fluxuate wildly from 
// current time (e.g. 13213) to zero, causing lots of strange visual behaviors based on the time float. This 
// way we maintain the same time float when slow or paused, thus no strange fluctuations
public class ShaderTimingManipulator : MonoBehaviour
{
    public VariableSlider gameSpeed;
    float currentTime = 0;
    Renderer r;
    MaterialPropertyBlock materialPropertyBlock;

    private void Start()
    {
        r = GetComponent<Renderer>();
        materialPropertyBlock = new MaterialPropertyBlock();

    }
    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime * gameSpeed.value;
        materialPropertyBlock.SetFloat("Time_", currentTime);
        r.SetPropertyBlock(materialPropertyBlock);
    }

    public float GetTime()
    {
        return currentTime;
    }

}
