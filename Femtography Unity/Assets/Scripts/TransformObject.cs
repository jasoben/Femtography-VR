using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformObject : MonoBehaviour
{
    public Particle particle;
    public float KineticSpeed { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        particle.speed = particle.normalSpeed * particle.playbackSpeed.Value;
        KineticSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        particle.speed = particle.normalSpeed * particle.playbackSpeed.Value;
        transform.Translate(Vector3.Scale(transform.forward, new Vector3(0,0, particle.speed * KineticSpeed)));
    }

    public void StartKineticMotion()
    {
        KineticSpeed = 1;
    }

}
