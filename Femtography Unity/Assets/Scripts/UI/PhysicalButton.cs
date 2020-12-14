using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalButton : MonoBehaviour
{
    public Material mouseOverMaterial;
    private Material startMaterial;
    // Start is called before the first frame update
    void Start()
    {
        startMaterial = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        GetComponent<Renderer>().material = mouseOverMaterial;
        Debug.Log("mouse over");
    }
    private void OnMouseExit()
    {
        GetComponent<Renderer>().material = startMaterial;
    }
}
