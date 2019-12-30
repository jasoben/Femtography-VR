using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuarkForce : MonoBehaviour
{
    public Vector3 forceOnQuark;
    private Color newColor;
    private GameObject opacityControllerObject;

    // Start is called before the first frame update
    void Start()
    {
        opacityControllerObject = GameObject.Find("OpacityControlObject"); 
        GetComponent<Rigidbody>().AddForce(forceOnQuark);
    }

    // Update is called once per frame
    void Update()
    {
        float newOpacity = opacityControllerObject.GetComponent<OpacityController>().SpinSphereOpacity;
        newColor = new Color(GetComponent<Renderer>().material.color.r, GetComponent<Renderer>().material.color.g, GetComponent<Renderer>().material.color.b, newOpacity);
        GetComponent<Renderer>().material.color = newColor;

        
    }
}
