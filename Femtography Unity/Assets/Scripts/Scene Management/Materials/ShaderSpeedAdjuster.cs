using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderSpeedAdjuster : MonoBehaviour
{
    public FloatReference playBackSpeed;
    public List<Material> materialsToAdjust;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Material material in materialsToAdjust)
        {
            material.SetFloat("Speed_", playBackSpeed.Value * 2);
        }
        
    }

    public float ChangeSpeed(float value)
    {
        return (value * playBackSpeed.Value);
    }
}
