using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealTimeReflection : MonoBehaviour
{
    ReflectionProbe reflectionProbe;
    int frame = 0;
    int frameToResetAt = 2;
    // Start is called before the first frame update
    void Start()
    {
        reflectionProbe = GetComponent<ReflectionProbe>();
    }

    // Update is called once per frame
    void Update()
    {
        frame++;
        if (frame > frameToResetAt)
        {
            reflectionProbe.RenderProbe();
            frame = 0;
        }
    }
}
