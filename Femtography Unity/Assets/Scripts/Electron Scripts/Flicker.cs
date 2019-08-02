using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour
{

    public Light electronLight;
    private float lightIntensity;
    private float lightIntensityDelta;
    public float lightIntensityDeltaCoefficient;

    // Start is called before the first frame update
    void Start()
    {
        lightIntensity = .5f;
        lightIntensityDelta = 1;
    }

    // Update is called once per frame
    void Update()
    {
    
        if (lightIntensity < 0.4f)
        {
            lightIntensityDelta = 1;
        }
        else if (lightIntensity > .6f)
        {
            lightIntensityDelta = -1;
        }

        lightIntensity += lightIntensityDelta * lightIntensityDeltaCoefficient;
        electronLight.intensity = lightIntensity;

    }

    public void ElectronDisapear()
    {
        Renderer[] theseRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer thisRenderer in theseRenderers)
        {
            thisRenderer.enabled = false;
        }
    }
}
