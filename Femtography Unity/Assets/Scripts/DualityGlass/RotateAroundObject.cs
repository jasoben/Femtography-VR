using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundObject : MonoBehaviour
{
    public GameObject centerObject;
    public float rotateAmount;
    // Start is called before the first frame update
    void Start()
    {
        
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
        
    }
}
