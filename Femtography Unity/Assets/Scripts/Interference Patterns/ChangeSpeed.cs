using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveMaker;

public class ChangeSpeed : MonoBehaviour
{
    WaveMakerSurface waveMakerSurface;
    public WaveMakerGOMover waveMakerGOMover;
    float waveMakerGoHeight;
    // Start is called before the first frame update
    void Start()
    {
        waveMakerSurface = GetComponent<WaveMakerSurface>();
        if (waveMakerGOMover != null)
            waveMakerGoHeight = waveMakerGOMover.translationDistance.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSpeedTweak(float newSpeed)
    {
        waveMakerSurface.speedTweak = newSpeed;
        if (waveMakerGOMover != null)
            waveMakerGOMover.translationDistance.y = waveMakerGoHeight / newSpeed;

    }
}
