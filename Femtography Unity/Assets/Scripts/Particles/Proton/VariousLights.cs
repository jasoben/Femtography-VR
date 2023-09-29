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
    bool lerpGoingUp = true;
    // Start is called before the first frame update
    void Start()
    {
        lights = new Light[] {light1, light2, light3};
        randomIntensity = Random.Range(intensity - intensityRange, intensity + intensityRange);
        light1.range = lightSize + 5; 
        light2.range = lightSize + 10; 
        light3.range = lightSize + 15; 
    }

    // Update is called once per frame
    void Update()
    {

        foreach (Light light in lights)
        {
            lerpAmount += flickerSpeed * Time.deltaTime;
            light.intensity = Mathf.Lerp(intensity, randomIntensity, lerpAmount);
        }
        Debug.Log(lerpAmount);
        
        if (lerpAmount > 1 && lerpGoingUp)
        {
            flickerSpeed = -flickerSpeed;
            lerpGoingUp = false;
        }

        else if (lerpAmount < 0 && !lerpGoingUp)
        {
            randomIntensity = Random.Range(intensity - intensityRange, intensity + intensityRange);
            flickerSpeed = -flickerSpeed;
            lerpGoingUp = true;
        }
    }
}
