using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectWiggler : MonoBehaviour, ISpeedController
{
    [SerializeField][Range(0,99)] float maxDistanceFromStart, speed;
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
        moveDirection = Random.insideUnitSphere;
    }

    // Update is called once per frame
    void Update()
    {
        localPosition = transform.localPosition;
        worldStartPosition = transform.parent.TransformPoint(localStartPosition);
        adjustedSpeed = speed * playBackSpeed.Value * Time.deltaTime;

        if (stayStill)
        {
            return;
        }

        transform.position += moveDirection * adjustedSpeed;
        float distanceFromStart = Vector3.Distance(transform.position, worldStartPosition);

        if (distanceFromStart > maxDistanceFromStart && !isMovingBack)
        {
            moveDirection = worldStartPosition - transform.position;
            moveDirection.Normalize();
            isMovingBack = true;
        }
        else if (distanceFromStart > maxDistanceFromStart * 2f)
        {
            transform.position = worldStartPosition;
        }

        else if (distanceFromStart < .5f && isMovingBack)
        {
            moveDirection = Random.insideUnitSphere;
            isMovingBack = false;
        }

        //if (Vector3.Distance(transform.position, worldStartPosition) > 3 * distanceFromStart)
        //{
        //    transform.position = worldStartPosition;
        //}

    }
}
