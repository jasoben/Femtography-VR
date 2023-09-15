using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightMover : MonoBehaviour
{
    [SerializeField] Vector3 startRotation, endRotation;
    [SerializeField] float rotationSpeed;

    Quaternion startRotQuat, endRotQuat;

    float lerpAmount;
    bool isFullyRotated;

    // Start is called before the first frame update
    void Start()
    {
        startRotQuat = Quaternion.Euler(startRotation);
        endRotQuat = Quaternion.Euler(endRotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFullyRotated)
        {
            lerpAmount -= rotationSpeed;
        } else lerpAmount += rotationSpeed;

        gameObject.transform.rotation = Quaternion.Lerp(startRotQuat, endRotQuat, lerpAmount);

        if (lerpAmount < .01f && isFullyRotated)
            isFullyRotated = false;
        else if (lerpAmount > .99f && !isFullyRotated)
            isFullyRotated = true;
        
    }
}
