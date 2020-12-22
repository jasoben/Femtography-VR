using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWiggler : MonoBehaviour
{
    // This wiggles UI around a bit to preserve the appearance of the liquid display

    Vector3 startPosition, endPosition, currentStartPosition;
    public float wiggleRange, wiggleSpeed, wiggleCoefficient;
    private Vector2 xBounds, yBounds, zBounds;
    float wiggleCounter;
    public bool useSphere = true;
    public bool WigglerPaused { get; set; }

    public BoxCollider boundingBox;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        currentStartPosition = transform.position;
        wiggleRange = wiggleRange * wiggleCoefficient;

        if (useSphere)
            endPosition = (Random.insideUnitSphere * wiggleRange) + startPosition;
        else
        {
            xBounds = new Vector2(boundingBox.bounds.center.x - boundingBox.bounds.extents.x, boundingBox.bounds.center.x + boundingBox.bounds.extents.x);
            yBounds = new Vector2(boundingBox.bounds.center.y - boundingBox.bounds.extents.y, boundingBox.bounds.center.y + boundingBox.bounds.extents.y);
            zBounds = new Vector2(boundingBox.bounds.center.z - boundingBox.bounds.extents.z, boundingBox.bounds.center.z + boundingBox.bounds.extents.z);

            endPosition = new Vector3(Random.Range(xBounds.x, xBounds.y),
                Random.Range(yBounds.x, yBounds.y),
                Random.Range(zBounds.x, zBounds.y));
        }

        wiggleCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!WigglerPaused)
        {
            transform.position = Vector3.Slerp(currentStartPosition, endPosition, wiggleCounter);
            wiggleCounter += wiggleSpeed;
        }

        if (wiggleCounter > 1)
        {
            if (useSphere)
                endPosition = (Random.insideUnitSphere * wiggleRange) + startPosition;
            else
            {
                endPosition = new Vector3(Random.Range(xBounds.x, xBounds.y),
                    Random.Range(yBounds.x, yBounds.y),
                    Random.Range(zBounds.x, zBounds.y));
            }

            currentStartPosition = transform.position;
            wiggleCounter = 0;
        }
    }

}
