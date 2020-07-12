using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MovementController
{
    public static List<TransformObject> TransformObjects { get; set; } = new List<TransformObject>();

    public static void MoveObjects()
    {
        foreach (TransformObject transformObject in TransformObjects)
        {
            transformObject.StartMoving();
        }
    }
    public static void StopMovingObjects()
    {
        foreach (TransformObject transformObject in TransformObjects)
        {
            transformObject.StopMoving();
        }
    }
}
