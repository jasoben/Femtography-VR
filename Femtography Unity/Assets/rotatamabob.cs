using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotatamabob : MonoBehaviour
{
    GameObject mainCamera;
    
    void Start()
    {
        mainCamera = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 fromTo = mainCamera.transform.position - gameObject.transform.position;
        Vector3 projectedVector = Vector3.ProjectOnPlane(fromTo, gameObject.transform.up);
        float angle = Vector3.Dot(projectedVector, gameObject.transform.right);
        gameObject.transform.Rotate(gameObject.transform.up, angle, Space.World);
        
    }
}
