using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerMover : MonoBehaviour
{

    private Renderer thisRenderer;
    // Start is called before the first frame update
    void Start()
    {
        thisRenderer = GetComponent<Renderer>();
        thisRenderer.enabled = false;
    }

    public void MakeVisible()
    {
        thisRenderer.enabled = true;
    }

    public void MakeInvisible()
    {
        thisRenderer.enabled = false;
    }
    
}
