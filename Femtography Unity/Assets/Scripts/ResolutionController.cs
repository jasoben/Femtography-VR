using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionController : MonoBehaviour
{
    public FloatReference resolution;
    public GameObject lightObject;
    public Light spotLight;
    public float multiplier, position;

    // Start is called before the first frame update
    void Start()
    {
        multiplier = 25;
        position = 55;
    }

    // Update is called once per frame
    void Update()
    {
        lightObject.transform.localPosition = new Vector3(0, position - resolution.Value * multiplier, 0);
    }
}
