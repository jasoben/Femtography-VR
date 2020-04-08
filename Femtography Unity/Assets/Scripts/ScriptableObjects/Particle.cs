﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class Particle : ScriptableObject
{
    public Material particleMaterial;
    public FloatReference opacity, playbackSpeed;
    public float speed, normalSpeed;
    public UnityEvent opacityChanged;
    private float bufferSpeed;
    public Shader transparentShader, opaqueShader;

    void OnEnable()
    {
        particleMaterial.SetVector("RandomSeed", new Vector4(Random.Range(0, 100), Random.Range(0, 100), Random.Range(0, 100), Random.Range(0, 100)));
    }

    public void Pause()
    {
        bufferSpeed = playbackSpeed.Value;
        playbackSpeed.variable.value = 0;
    }

    public void Play()
    {
        if (playbackSpeed.Value == 0)
        {
            playbackSpeed.variable.value = bufferSpeed;
        }
        speed = playbackSpeed.Value * normalSpeed;
    }

}
