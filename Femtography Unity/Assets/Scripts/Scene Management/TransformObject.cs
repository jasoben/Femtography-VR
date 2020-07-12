using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformObject : MonoBehaviour
{
    public Particle particle;
    public bool IsTransformActive { get; set; }
    public bool CanBeMoved { get; set; } = true;

    private void Start()
    {
        MovementController.TransformObjects.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsTransformActive && CanBeMoved)
            transform.Translate(Vector3.Scale(transform.forward, new Vector3(0,0, particle.transformSpeed * particle.playbackSpeed.Value)));
    }

    public void StartMoving()
    {
        IsTransformActive = true;
    }
    public void StopMoving()
    {
        IsTransformActive = false;
    }

    public void NoLongerMoveable()
    {
        CanBeMoved = false;
    }
    public void MakeMoveable()
    {
        CanBeMoved = true;
    }
}
