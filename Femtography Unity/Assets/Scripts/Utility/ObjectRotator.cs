using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour, ISpeedController
{
    [SerializeField] Vector3 rotationSpeed;

    FloatReference playBackSpeed;

    public void RequestInjection(object obj, MonoBehaviour monoBehaviour)
    {
        DependencyInjector.InvokeInjectorEvent(this, this);
    }

    public void SetSpeedReference(FloatReference _playBackSpeed)
    {
        playBackSpeed = _playBackSpeed;
    }

    
    // Start is called before the first frame update
    void Start()
    {
        RequestInjection(this, this);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationSpeed * playBackSpeed.Value * Time.deltaTime);
    }
}
