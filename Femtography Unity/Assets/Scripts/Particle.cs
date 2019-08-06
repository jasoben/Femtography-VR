using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class Particle : ScriptableObject
{
    public Material particleMaterial, particleOpqMaterial, particleFadeMaterial;

    [Range(0, 1)]
    public float opacity;
    public float speed, normalSpeed;

    public UnityEvent opacityChanged;

    void Start()
    {
        particleMaterial = particleOpqMaterial;
    }

    void OnValidate()
    {
        opacityChanged.Invoke();
        if (opacity > .8f)
        {
            particleMaterial = particleOpqMaterial;
        }
        else
        {
            particleMaterial = particleFadeMaterial;
            particleMaterial.SetColor("_Color", new Color(particleMaterial.color.r, particleMaterial.color.g, particleMaterial.color.b, opacity));
        }

        
    }
}
