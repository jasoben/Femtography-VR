using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThePlayerController : MonoBehaviour
{
    public Particle particle;
    // Start is called before the first frame update
    void Start()
    {
        particle.speed = 0; 
        particle.normalSpeed = 1; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
