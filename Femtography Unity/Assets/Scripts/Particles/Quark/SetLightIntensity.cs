using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SetLightIntensity : MonoBehaviour
{

    public Particle particle;
    public float rangeMultiplier, intensityMultiplier, rangeAddition, intensityAddition;

    void Start()
    {
        rangeMultiplier = 3.12f;
        intensityMultiplier = 1.24f;
        rangeAddition = intensityAddition = .28f;
    }
    // Update is called once per frame
    private void Update()
    {
        GetComponent<Light>().range = rangeMultiplier / (particle.opacity.Value + rangeAddition);
        GetComponent<Light>().intensity = (particle.opacity.Value + intensityAddition) * intensityMultiplier;
    }
}
