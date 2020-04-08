using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerMover : MonoBehaviour
{

    private Renderer thisRenderer;
    // Start is called before the first frame update
    private void Awake()
    {
        thisRenderer = GetComponentInChildren<Renderer>();
        thisRenderer.enabled = false;
    }
    void Start()
    {
    }

    public void MakeVisible()
    {
        if (thisRenderer != null)
            thisRenderer.enabled = true;
    }

    public void MakeInvisible()
    {
        thisRenderer.enabled = false;
    }
    
}
