﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableController : MonoBehaviour
{
    public GameObject objectToControl, startPosition, endPosition, sphereToControl, lightToControl;
    private float variableToControl;
    private float distanceMeasure;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distanceMeasure = Vector3.Distance(transform.position, startPosition.transform.position);
        objectToControl.GetComponent<EllipticalOrbit>().MagnitudeFraction = distanceMeasure * 15;
        sphereToControl.GetComponent<SphereCreator>().sphereSize = distanceMeasure * 15;
        lightToControl.GetComponent<VariousLights>().lightSize = distanceMeasure * 5;
    }
}