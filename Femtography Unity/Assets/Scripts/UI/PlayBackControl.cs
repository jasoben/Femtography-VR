using System.Collections;
using System.Collections.Generic;

public static class PlayBackControl 
{
    public static bool isPlaying;

    static PlayBackControl()
    {
        isPlaying = false;
    }

    public static void StartPlaying()
    {
        isPlaying = true;
    }
    public static void StopPlaying()
    {
        isPlaying = false;
    }

}
