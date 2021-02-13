using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLabel : MonoBehaviour
{
    public Vector3 rotationAxis;
    public FloatReference gameSpeed;
    float clampedGameSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        clampedGameSpeed = Mathf.Clamp(gameSpeed.Value, .3f, 1f);
        transform.Rotate(rotationAxis * clampedGameSpeed);
    }

    public void UpdateScale(float newScale)
    {
        transform.localScale = new Vector3(newScale, newScale, newScale);
    }
}
