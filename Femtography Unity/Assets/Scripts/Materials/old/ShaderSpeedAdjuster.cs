using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderSpeedAdjuster : MonoBehaviour
{
    public FloatReference playBackSpeed;
    public List<Material> materialsToAdjust;
    float clampedSpeedValue; // We clamp it because we don't want most shaders to reduce to zero or it would look 
    // strange

    // Start is called before the first frame update
    void Start()
    {
        SetNewSpeedValue();
    }
    public void SetNewSpeedValue()
    {
        //are faster than 1
        foreach (Material material in materialsToAdjust)
        {
            material.SetFloat("Speed_", playBackSpeed.Value);
        }
    }

    public float ChangeSpeed(float value)
    {
        return (value * playBackSpeed.Value);
    }
}
