using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpacityController : MonoBehaviour
{

    public float GrowSphereOpacity;
    public float SpinSphereOpacity;
    // Start is called before the first frame update
    void Start()
    {
        GrowSphereOpacity = SpinSphereOpacity = 1f;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeOpacity(float newOpacity, SphereType thisSphereType)
    {
        if (thisSphereType == SphereType.GrowSphere)
            GrowSphereOpacity = newOpacity;
        else if (thisSphereType == SphereType.SpinSphere)
            SpinSphereOpacity = newOpacity;
    }
}

public enum SphereType
{
    GrowSphere,
    SpinSphere
}
