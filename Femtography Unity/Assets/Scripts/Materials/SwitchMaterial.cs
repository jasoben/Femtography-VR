using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMaterial : MonoBehaviour
{
    public Material[] materials;

    public void SwitchToMaterial(int materialNumber)
    {
        GetComponent<Renderer>().material = materials[materialNumber];
    }
}
