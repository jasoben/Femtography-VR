using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePoints : MonoBehaviour
{
    public GameObject upPoint, leftPoint, forwardPoint;
    private List<GameObject> QPoints;
    private List<GameObject> LPoints;
    private List<GameObject> RPoints;

    // Start is called before the first frame update
    void Start()
    {
        QPoints = new List<GameObject>();
        LPoints = new List<GameObject>();
        RPoints = new List<GameObject>();

        GameObject[] qObjects = GameObject.FindGameObjectsWithTag("Q-Squared");
        GameObject[] lObjects = GameObject.FindGameObjectsWithTag("L");
        GameObject[] rObjects = GameObject.FindGameObjectsWithTag("R");
        
        foreach (GameObject thisObject in qObjects)
        {
            QPoints.Add(thisObject);
        }
        foreach (GameObject thisObject in lObjects)
        {
            LPoints.Add(thisObject);
        }
        foreach (GameObject thisObject in rObjects)
        {
            RPoints.Add(thisObject);
        }

        foreach (GameObject thisObject in QPoints)
        {
            GameObject.Instantiate(upPoint, thisObject.transform.position, Quaternion.identity, thisObject.transform);
        }
        foreach (GameObject thisObject in LPoints)
        {
            GameObject.Instantiate(leftPoint, thisObject.transform.position, Quaternion.identity, thisObject.transform);
        }
        foreach (GameObject thisObject in RPoints)
        {
            GameObject.Instantiate(forwardPoint, thisObject.transform.position, Quaternion.identity, thisObject.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
