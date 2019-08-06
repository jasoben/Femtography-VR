﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformObject : MonoBehaviour
{
    public Particle particle;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.Scale(transform.forward, new Vector3(0,0, particle.speed * particle.normalSpeed)));
    }

    public void StopMoving()
    {
        particle.speed = 0;
    }

}
