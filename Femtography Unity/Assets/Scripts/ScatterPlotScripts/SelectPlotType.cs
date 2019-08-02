using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPlotType : MonoBehaviour
{
    public GameObject upPoint, leftPoint, forwardPoint;
    private GameObject[] allPoints;
    private GameObject availablePoint;

    // Start is called before the first frame update
    void Start()
    {
        allPoints = new GameObject[3];
        allPoints[0] = upPoint;
        allPoints[1] = leftPoint;
        allPoints[0] = forwardPoint;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
