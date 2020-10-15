using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundObject : MonoBehaviour
{
    public GameObject centerObject;
    public float rotateAmount;

    bool switchObjects = true;

    public List<GameObject> platypusObjects, particleObjects; 
    // Start is called before the first frame update
    void Start()
    {
        SwitchTheObjects();
    }

    void SwitchTheObjects()
    {
        foreach (GameObject thisObject in platypusObjects)
        {
            thisObject.SetActive(switchObjects);
        }
        foreach (GameObject thisObject in particleObjects)
        {
            if (thisObject.GetComponent<CloudCreator>() != null)
            {
                thisObject.GetComponent<CloudCreator>().StopAllCoroutines();
            }
            thisObject.SetActive(!switchObjects);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.RotateAround(centerObject.transform.position, Vector3.up, rotateAmount);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            transform.RotateAround(centerObject.transform.position, Vector3.up, -rotateAmount);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            switchObjects = !switchObjects;
            SwitchTheObjects();
        }
        
    }
}
