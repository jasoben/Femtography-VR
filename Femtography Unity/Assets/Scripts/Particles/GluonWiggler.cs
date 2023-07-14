using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GluonWiggler : MonoBehaviour
{
    [SerializeField] float distanceFromStart, speed;

    Vector3 startPosition, moveDirection;
    private Quaternion startRotation;
    private Vector3 moveRotation;
    [SerializeField] private float resetDistance;
    [SerializeField] bool stayStill;

    GameObject childBone;

    // Start is called before the first frame update

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        moveDirection = GetNewDirection(-1, 1);
        moveRotation = GetNewDirection(-1, 1);
        if (transform.childCount > 0)
            childBone = transform.GetChild(0).gameObject;
    }
    Vector3 GetNewDirection(float bottom, float top)
    {
        return new Vector3(Random.Range(bottom,top), Random.Range(bottom,top), Random.Range(bottom,top));
    }

    // Update is called once per frame
    void Update()
    {
        if (stayStill)
        {
            transform.position = startPosition;
            return;
        }
        transform.position += moveDirection / 1000 * speed;
        //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x + moveRotation.x, 
        //    transform.rotation.eulerAngles.y + moveRotation.y,
        //    transform.rotation.eulerAngles.z + moveRotation.z);

        if (Vector3.Distance(transform.position, startPosition) > distanceFromStart)
        {
            moveDirection = startPosition - transform.position;
            transform.position += moveDirection / 500 * speed;
        }
        if (Vector3.Distance(transform.position, startPosition) < resetDistance)
        {
            moveDirection = GetNewDirection(-1, 1);
        }

        if (Quaternion.Angle(transform.rotation, startRotation) > 10)
        {
            moveRotation = -moveRotation;
        }
    }
}
