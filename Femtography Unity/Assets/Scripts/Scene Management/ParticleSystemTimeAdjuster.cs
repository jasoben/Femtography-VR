using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemTimeAdjuster : MonoBehaviour
{
    public FloatReference playBackSpeed;
    ParticleSystem thisParticleSystem;
    // Start is called before the first frame update
    void Start()
    {
        thisParticleSystem = GetComponent<ParticleSystem>(); 
    }

    // Update is called once per frame
    void Update()
    {
        var main = thisParticleSystem.main;
        main.simulationSpeed = playBackSpeed.Value;
    }
}
