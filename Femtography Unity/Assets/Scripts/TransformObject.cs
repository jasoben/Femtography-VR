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
        particle.speed = 1;
        KineticSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.Scale(transform.forward, new Vector3(0,0, particle.speed * particle.normalSpeed * KineticSpeed)));
    }

    public void StopMoving()
    {
        particle.speed = 0;
    }

    public void StartMoving()
    {

    }

    public void StartKineticMotion()
    {
        KineticSpeed = 1;
    }

}
