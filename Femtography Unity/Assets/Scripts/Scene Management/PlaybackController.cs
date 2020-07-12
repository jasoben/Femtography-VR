using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaybackController : MonoBehaviour
{
    public FloatReference playbackSpeed;
    private float savedSpeed;

    void Start()
    {
    }

    void Update()
    {
    }
    public void Play()
    {
        
    }
    public void Pause()
    {
        savedSpeed = playbackSpeed.Value;
        playbackSpeed.Value = 0;
    }
}
