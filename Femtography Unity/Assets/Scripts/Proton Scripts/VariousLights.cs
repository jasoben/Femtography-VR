using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariousLights : MonoBehaviour
{
    public GameObject light1, light2, light3;
    public float lightSize;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        light1.GetComponent<Light>().range = lightSize + 5; 
        light2.GetComponent<Light>().range = lightSize + 10; 
        light3.GetComponent<Light>().range = lightSize + 15; 
    }
}
