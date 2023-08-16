using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariousLights : MonoBehaviour
{
    public Light light1, light2, light3;
    Light[] lights;
    public float lightSize;
    [SerializeField] float intensity;
    [SerializeField] float flickerSpeed;
    [SerializeField] float intensityRange;
    float randomIntensity;
    float lerpAmount = 0;
    // Start is called before the first frame update
    void Start()
    {
        lights = new Light[] {light1, light2, light3};
        randomIntensity = Random.Range(intensity - intensityRange, intensity + intensityRange);
    }

    // Update is called once per frame
    void Update()
    {
        light1.range = lightSize + 5; 
        light2.range = lightSize + 10; 
        light3.range = lightSize + 15; 

        foreach (Light light in lights)
        {
            lerpAmount += flickerSpeed;
            light.intensity = Mathf.Lerp(intensity, randomIntensity, lerpAmount);
        }
        
        if (lerpAmount > 1)
        {
            flickerSpeed = -flickerSpeed;
        }

        else if (lerpAmount < 0)
        {
            randomIntensity = Random.Range(intensity - intensityRange, intensity + intensityRange);
            flickerSpeed = -flickerSpeed;
        }
    }
}
