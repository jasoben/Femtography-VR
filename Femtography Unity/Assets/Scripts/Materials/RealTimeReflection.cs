using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealTimeReflection : MonoBehaviour
{
    ReflectionProbe reflectionProbe;
    // Start is called before the first frame update
    void Start()
    {
        reflectionProbe = GetComponent<ReflectionProbe>();
    }

    // Update is called once per frame
    void Update()
    {
        reflectionProbe.RenderProbe();
    }
}
