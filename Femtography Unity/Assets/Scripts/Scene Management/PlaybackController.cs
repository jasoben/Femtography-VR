using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaybackController : MonoBehaviour
{
    public FloatReference playbackSpeed;
    private float savedSpeed;

    void Start()
    {
        savedSpeed = 1;
    }

    void Update()
    {
    }
    public void Play()
    {
        playbackSpeed.Value = savedSpeed;
    }
    public void Pause()
    {
        if (playbackSpeed.Value > 0)
            savedSpeed = playbackSpeed.Value;
        playbackSpeed.Value = 0;
    }
}
