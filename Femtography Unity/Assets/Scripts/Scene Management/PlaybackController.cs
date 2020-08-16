using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaybackController : MonoBehaviour
{
    public FloatReference playbackSpeed;
    private float savedSpeed;
    public GlobalBool pauseAtEvents;
    public GameEvent pauseEverything;

    void Start()
    {
        savedSpeed = 1;
        pauseAtEvents.boolValue = true;
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

    public void PauseAtEvents()
    {
        if (pauseAtEvents.boolValue)
        {
            pauseEverything.Raise();
        }
    }

    public void ChangePlayBackSpeed(float speedValue)
    {
        savedSpeed = speedValue;
        if (playbackSpeed.Value == 0)
        {
            savedSpeed = speedValue;
        } else playbackSpeed.Value = speedValue;
    }

}
