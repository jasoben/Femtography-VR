using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectWiggler : MonoBehaviour, ISpeedController
{
    [SerializeField] float distanceFromStart, speed;
    float adjustedSpeed;

    Vector3 localStartPosition, moveDirection, moveRotation, localPosition, worldStartPosition;
    [SerializeField] bool stayStill;

    bool isMovingBack = false;

    FloatReference playBackSpeed;

    public void RequestInjection(object obj, MonoBehaviour monoBehaviour)
    {
        DependencyInjector.InvokeInjectorEvent(obj, monoBehaviour);
    }

    public void SetSpeedReference(FloatReference _playBackSpeed)
    {
        playBackSpeed = _playBackSpeed;
    }

    // Start is called before the first frame update

    void Start()
    {
        RequestInjection(this, this);
        localStartPosition = transform.localPosition;
        moveDirection = Random.insideUnitSphere * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        localPosition = transform.localPosition;
        worldStartPosition = transform.parent.TransformPoint(localStartPosition);
        adjustedSpeed = speed * playBackSpeed.Value;

        if (stayStill)
        {
            return;
        }

        transform.position += moveDirection * adjustedSpeed;

        if (Vector3.Distance(transform.position, worldStartPosition) > distanceFromStart && 
            !isMovingBack)
        {
            moveDirection = worldStartPosition - transform.position;
            isMovingBack = true;
        }
        else if (Vector3.Distance(transform.position, worldStartPosition) > 2 * distanceFromStart)
        {
            transform.position = worldStartPosition;
        }

        else if (Vector3.Distance(transform.position, worldStartPosition) < .5f &&
            isMovingBack)
        {
            moveDirection = Random.insideUnitSphere * Time.deltaTime;
            isMovingBack = false;
        }


    }
}
