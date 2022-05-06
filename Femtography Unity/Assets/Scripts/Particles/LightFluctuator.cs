using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFluctuator : MonoBehaviour
{
    private Light light;
    private float initialIntensity, somewhatRandomIntensityChange;
    private float deltaIntensity;
    [SerializeField]
    [Range(0, 10)]
    private float fluctuationAmount;

    [SerializeField]
    [Range(0, 10)]
    private float fluctuationSpeed;

    [SerializeField]
    [Range(0, 5)]
    private float randomNess;

    private float coefficientOfFluctuation = .005f;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        initialIntensity = light.intensity;
        somewhatRandomIntensityChange = fluctuationAmount + Random.Range(0, randomNess);
        deltaIntensity = coefficientOfFluctuation * fluctuationSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        light.intensity = light.intensity + deltaIntensity;

        //going up, switch to down
        if (light.intensity - initialIntensity > somewhatRandomIntensityChange)
        {
            somewhatRandomIntensityChange = fluctuationAmount + Random.Range(0, randomNess);
            deltaIntensity = -Mathf.Abs(deltaIntensity);
        }
        //going down, switch to up
        else if (light.intensity - initialIntensity < -somewhatRandomIntensityChange || light.intensity == 0)
        {
            somewhatRandomIntensityChange = fluctuationAmount + Random.Range(0, randomNess);
            deltaIntensity = Mathf.Abs(deltaIntensity);
        }
        
    }

}
