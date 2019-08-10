using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionController : MonoBehaviour
{
    public FloatReference resolution;
    public List<Material> resolutionStages;
    public GameObject photoFilm;
    private int particlesHit;

    // Start is called before the first frame update
    void Start()
    {
        particlesHit = 0;
    }

    // Update is called once per frame
    void Update()
    {
        photoFilm.GetComponent<Renderer>().material = resolutionStages[(int)(4 * resolution.Value)];
        float ratio = particlesHit / 10f;
        photoFilm.GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 1, 1, ratio));
    }

    public void IncreaseParticles()
    {
        particlesHit++;
    }
}
