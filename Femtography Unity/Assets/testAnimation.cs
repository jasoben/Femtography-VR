﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testAnimation : MonoBehaviour
{
    public Animator cubeAnimator, photonAnimator;
    public Particle particle;
    public TransformObject photonCollider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            cubeAnimator.Play("Spin", 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            photonAnimator.Play("LaunchPhoton", 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            photonAnimator.speed = 0;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
        }
    }
}
