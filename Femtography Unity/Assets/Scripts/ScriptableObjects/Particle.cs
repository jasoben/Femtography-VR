using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class Particle : ScriptableObject
{
    public Material particleMaterialOpaque, particleMaterialTransparent;
    public FloatReference opacity, playbackSpeed;
    public float transformSpeed;
    public UnityEvent opacityChanged;

}
