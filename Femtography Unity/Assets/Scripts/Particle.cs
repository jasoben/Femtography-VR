using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class Particle : ScriptableObject
{
    public Material particleMaterial, particleOpqMaterial, particleFadeMaterial;

    public FloatReference opacity;
    public float speed, normalSpeed;
    public UnityEvent opacityChanged;
    private float bufferSpeed;

    void Start()
    {
        particleMaterial = particleOpqMaterial;
    }

    public void Pause()
    {
        bufferSpeed = speed;
        speed = 0;
    }

    public void Play()
    {
        speed = bufferSpeed;
    }

}
