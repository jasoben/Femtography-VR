using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{

    ParticleSystem thisParticleSystem;

    // Start is called before the first frame update
    void Start()
    {
        thisParticleSystem = GetComponent<ParticleSystem>();
        thisParticleSystem.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartMovingParticles()
    {
        thisParticleSystem.Play();
    }
    public void PauseMovingParticles()
    {
        thisParticleSystem.Pause();
    }
}
