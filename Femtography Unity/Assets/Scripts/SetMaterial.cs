using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMaterial : MonoBehaviour
{
    public Particle particle;
    private bool opaqueOrNot;

    // Start is called before the first frame update
    void Start()
    {
        opaqueOrNot = true;
        //foreach(ParticleSystemRenderer thisPSR in GetComponentsInChildren<ParticleSystemRenderer>())
        //{
        //    thisPSR.material = particle.particleMaterial;
        //}
        foreach(Renderer thisRenderer in GetComponentsInChildren<Renderer>())
        {
            thisRenderer.material = particle.particleMaterial;
        }

        GetComponentInChildren<Light>().color = particle.particleOpqMaterial.color;
        
    }
    void Update()
    {
        if (particle.opacity.Value > .8f && !opaqueOrNot)
        {
            opaqueOrNot = true;
            particle.particleMaterial = particle.particleOpqMaterial;
            ChangeMaterial();
        } 
        else if (particle.opacity.Value <= .8f && opaqueOrNot)
        {
            opaqueOrNot = false;
            particle.particleMaterial = particle.particleFadeMaterial;
            ChangeMaterial();
        } 
        particle.particleMaterial.SetColor("_Color", new Color(particle.particleMaterial.color.r, particle.particleMaterial.color.g, particle.particleMaterial.color.b, particle.opacity.Value));

    }
    void ChangeMaterial()
    {
        //foreach(ParticleSystemRenderer thisPSR in GetComponentsInChildren<ParticleSystemRenderer>())
        //{
        //    thisPSR.material = particle.particleMaterial;
        //}
        foreach(Renderer thisRenderer in GetComponentsInChildren<Renderer>())
        {
            thisRenderer.material = particle.particleMaterial;
        }
     }

}
