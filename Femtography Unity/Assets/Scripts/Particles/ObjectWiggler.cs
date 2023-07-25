using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectWiggler : MonoBehaviour
{
    [SerializeField] float distanceFromStart, speed;

    Vector3 startPosition, moveDirection;
    private Vector3 moveRotation;
    [SerializeField] bool stayStill;

    bool isMovingBack = false;

    // Start is called before the first frame update

    void Start()
    {
        startPosition = transform.position;
        moveDirection = Random.insideUnitSphere;
    }

    // Update is called once per frame
    void Update()
    {
        if (stayStill)
        {
            transform.position = startPosition;
            return;
        }

        transform.position += moveDirection * speed;

        if (Vector3.Distance(transform.position, startPosition) > distanceFromStart && 
            !isMovingBack)
        {
            moveDirection = startPosition - transform.position;
            isMovingBack = true;
        }

        else if (Vector3.Distance(transform.position, startPosition) < .5f &&
            isMovingBack)
        {
            moveDirection = Random.insideUnitSphere;
            isMovingBack = false;
        }


    }
}
