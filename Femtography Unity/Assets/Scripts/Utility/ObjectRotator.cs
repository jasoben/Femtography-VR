using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour, ISpeedController
{
    [SerializeField] Vector3 rotationSpeed;

    FloatReference playBackSpeed;
    public void SetSpeedReference(FloatReference _playBackSpeed)
    {
        playBackSpeed = _playBackSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationSpeed * playBackSpeed.Value);
    }
}
