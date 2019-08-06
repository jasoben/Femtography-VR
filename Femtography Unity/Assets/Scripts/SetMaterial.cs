using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMaterial : MonoBehaviour
{
    public Particle particle;

    // Start is called before the first frame update
    void Start()
    {
        foreach(ParticleSystemRenderer thisPSR in GetComponentsInChildren<ParticleSystemRenderer>())
        {
            thisPSR.material = particle.particleMaterial;
        }
        particle.opacityChanged.AddListener(ChangeMaterial);
        
    }
    void ChangeMaterial()
    {
        foreach(ParticleSystemRenderer thisPSR in GetComponentsInChildren<ParticleSystemRenderer>())
        {
            thisPSR.material = particle.particleMaterial;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
