using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableController : MonoBehaviour
{
    public GameObject objectToControl, startPosition, endPosition;
    private float distanceMeasure, totalDistance;

    // Start is called before the first frame update
    void Start()
    {
        totalDistance = Vector3.Distance(startPosition.transform.position, endPosition.transform.position);
        
    }

    // Update is called once per frame
    void Update()
    {
        distanceMeasure = Vector3.Distance(transform.position, startPosition.transform.position);
        float ratioDistance = distanceMeasure / totalDistance;
        objectToControl.GetComponent<OpacityController>().GrowSphereOpacity = 1 - ratioDistance;
        objectToControl.GetComponent<OpacityController>().SpinSphereOpacity = ratioDistance;

    }
}
