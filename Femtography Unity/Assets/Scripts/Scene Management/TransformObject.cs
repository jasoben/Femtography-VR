using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformObject : MonoBehaviour
{
    public Particle particle;
    public float KineticSpeed;
    public GlobalBool isPlaying;

    // Start is called before the first frame update
    void Start()
    {
        particle.speed = particle.normalSpeed * particle.playbackSpeed.Value;
        if (KineticSpeed == 1)
            KineticSpeed = 1;
        else
            KineticSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        particle.speed = particle.normalSpeed * particle.playbackSpeed.Value;
        if (isPlaying.boolValue)
            transform.Translate(Vector3.Scale(transform.forward, new Vector3(0,0, particle.speed * KineticSpeed)));
    }

    public void StartKineticMotion()
    {
        KineticSpeed = 1;
    }

}
