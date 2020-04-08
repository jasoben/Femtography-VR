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
    }
    void Update()
    {
        particleMaterial = particle.particleMaterial;
        GetComponent<Renderer>().material = particleMaterial;
    }

    public void SwitchShaderTransparent()
    {
        particleMaterial.shader = particle.transparentShader;
        particleMaterial.renderQueue = 4000;
    }

    public void SwitchShaderOpaque()
    {
        particleMaterial.shader = particle.opaqueShader;
        particleMaterial.renderQueue = 3000;
    }
}
