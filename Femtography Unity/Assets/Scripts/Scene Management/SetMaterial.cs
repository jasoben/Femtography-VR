using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMaterial : MonoBehaviour
{

    private Material particleMaterial;
    public Particle particle;
    // Start is called before the first frame update
    void Start()
    {
        particleMaterial = particle.particleMaterialTransparent;
    }
    void Update()
    {
        GetComponent<Renderer>().material = particleMaterial;
    }

    public void SwitchShaderTransparent()
    {
        particleMaterial = particle.particleMaterialOpaque;
        particleMaterial.SetFloat("Opacity", .2f);
        particleMaterial.SetFloat("HoleOpacity", .2f);
    }

    public void SwitchShaderOpaque()
    {
        particleMaterial = particle.particleMaterialOpaque;
        particleMaterial.SetFloat("Opacity", 1f);
        particleMaterial.SetFloat("HoleOpacity", .8f);
    }
}
