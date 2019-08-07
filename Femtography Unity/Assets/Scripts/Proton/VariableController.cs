using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableController : MonoBehaviour
{
    public GameObject startPosition, endPosition;
    private float distanceMeasure, totalDistance;
    public FloatReference QSlider;

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
        QSlider.variable.value = ratioDistance;
    }

    public void MoveToBottom()
    {
        transform.position = startPosition.transform.position;
    }
    public void MoveToTop()
    {
        transform.position = endPosition.transform.position;
    }
}
