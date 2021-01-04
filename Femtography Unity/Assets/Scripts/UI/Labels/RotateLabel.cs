using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLabel : MonoBehaviour
{
    public Vector3 rotationAxis;
    public FloatReference gameSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationAxis * gameSpeed.Value);
    }

    public void UpdateScale(float newScale)
    {
        transform.localScale = new Vector3(newScale, newScale, newScale);
    }
}
