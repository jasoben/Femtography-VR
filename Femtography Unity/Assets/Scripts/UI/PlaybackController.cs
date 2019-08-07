using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaybackController : MonoBehaviour
{
    public List<Particle> particles;
    private bool hasAnimation, hasParticleSystem;
    private Animator animator;
    private ParticleSystem particleSystem;

    void Start()
    {
        if (GetComponent<Animator>() != null)
        {
            animator = GetComponent<Animator>();
            hasAnimation = true;
        }
        if (GetComponent<ParticleSystem>() != null)
        {
            particleSystem = GetComponent<ParticleSystem>();
            hasParticleSystem = true;
        }

    }
    public void Play()
    {
        if (hasAnimation)
            animator.speed = 1;
        if (hasParticleSystem)
            particleSystem.Play();
        if (particles.Count > 0)
        {
            foreach (Particle thisParticle in particles)
            {
                thisParticle.Play();
            }
        }
    }
    public void Pause()
    {
        if (hasAnimation)
            animator.speed = 0;
        if (hasParticleSystem)
            particleSystem.Pause();
        if (particles.Count > 0)
        {
            foreach (Particle thisParticle in particles)
            {
                thisParticle.Pause();
            }
        }
    }
        
}
