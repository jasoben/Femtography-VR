using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jiggler : MonoBehaviour
{
    public float jiggleAmount, lerpAmount;
    private Vector3 newRandomLocation;
    public FloatReference playbackSpeed, q2;
    public bool insideSphere;
    // Start is called before the first frame update
    void Start()
    {
        newRandomLocation = (insideSphere ? Random.insideUnitSphere : Random.onUnitSphere) * jiggleAmount * q2.Value;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.localPosition, newRandomLocation) > .2f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, newRandomLocation, lerpAmount * playbackSpeed.Value);
        }
        else
        {
            newRandomLocation = (insideSphere ? Random.insideUnitSphere : Random.onUnitSphere) * jiggleAmount * (q2.Value+.2f);
        }
    }
}
