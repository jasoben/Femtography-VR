using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FlashingController
{
    public static float FlashLerp { get; set; }
    static float flashSpeed = .005f;
    static int upOrDown;

    public static void Flashing()
    {
        //Change the lerp value between the colors to produce a flashing effect
        if (FlashLerp > 1)
        {
            upOrDown = -1;
        }             
        else if (FlashLerp <= 0)
        {
            upOrDown = 1;
        }

        FlashLerp += flashSpeed * upOrDown;
    }
}
