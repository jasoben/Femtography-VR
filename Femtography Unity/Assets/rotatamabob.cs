using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotatamabob : MonoBehaviour
{
    public GameObject camera;
    // Start is called before the first frame update
    private Vector3 forwardPhoton;
    
    void Start()
    {
        camera = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 fromTo = camera.transform.position - gameObject.transform.position;
        Vector3 projectedVector = Vector3.ProjectOnPlane(fromTo, gameObject.transform.forward);
        float angle = Vector3.Dot(projectedVector, gameObject.transform.up);
        gameObject.transform.Rotate(gameObject.transform.forward, angle, Space.World);
        
    }
}
