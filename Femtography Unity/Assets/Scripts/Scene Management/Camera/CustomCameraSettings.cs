using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCameraSettings : MonoBehaviour
{
    float[] customDistances = new float[32];

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 31; i++)
        { 
            customDistances[i] = 500; 
        }

        Camera.main.layerCullDistances = customDistances;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
