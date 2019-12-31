using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaybackController : MonoBehaviour
{
    public List<Particle> particles;
    private bool hasAnimation, hasParticleSystem;
    private Animator animator;
    public FloatReference playbackSpeed;
    public GlobalBool isPlaying;

    void Start()
    {
        if (GetComponent<Animator>() != null)
        {
            animator = GetComponent<Animator>();
            hasAnimation = true;
        }
    }

    void Update()
    {
        if (playbackSpeed.Value > 0 && isPlaying.boolValue)
        {
            if (hasAnimation)
                animator.speed = playbackSpeed.Value;
            }
    }
    public void Play()
    {
        isPlaying.boolValue = true;
        if (hasAnimation)
            animator.speed = playbackSpeed.Value;
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
        isPlaying.boolValue = false;
        if (hasAnimation)
            animator.speed = 0;
        if (particles.Count > 0)
        {
            foreach (Particle thisParticle in particles)
            {
                thisParticle.Pause();
            }
        }
    }
}
