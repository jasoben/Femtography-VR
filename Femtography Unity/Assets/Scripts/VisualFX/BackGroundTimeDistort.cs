using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundTimeDistort : MonoBehaviour
{
    Material backgroundMaterial;
    public FloatReference simSpeed;
    float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        backgroundMaterial = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = currentTime + simSpeed.Value * Time.deltaTime;
        backgroundMaterial.SetFloat("Time_", currentTime); // .1f because we want it to be slow even at max speed
    }
}
