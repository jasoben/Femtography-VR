using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWiggler : MonoBehaviour
{
    // This wiggles UI around a bit to preserve the appearance of the liquid display

    Vector3 startPosition, endPosition, currentStartPosition, currentRelativePosition;
    public float wiggleRange, wiggleSpeed, wiggleCoefficient;
    private Vector2 xBounds, yBounds, zBounds;
    float wiggleCounter;
    public bool useSphere = true;
    public bool WigglerPaused { get; set; }

    public BoxCollider boundingBox;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.localPosition;
        currentStartPosition = transform.localPosition;
        wiggleRange = wiggleRange * wiggleCoefficient;

        if (useSphere)
            endPosition = (Random.insideUnitSphere * wiggleRange) + startPosition;
        else
        {
            Vector3 relativeBoundsCenter = transform.InverseTransformPoint(boundingBox.bounds.center);
            xBounds = new Vector2(relativeBoundsCenter.x - boundingBox.bounds.extents.x, relativeBoundsCenter.x + boundingBox.bounds.extents.x);
            yBounds = new Vector2(relativeBoundsCenter.y - boundingBox.bounds.extents.y, relativeBoundsCenter.y + boundingBox.bounds.extents.y);
            zBounds = new Vector2(relativeBoundsCenter.z - boundingBox.bounds.extents.z, relativeBoundsCenter.z + boundingBox.bounds.extents.z);

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
            currentRelativePosition = Vector3.Slerp(currentStartPosition, endPosition, wiggleCounter);
            transform.position = transform.parent.TransformPoint(currentRelativePosition);
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

            currentStartPosition = transform.localPosition;
            wiggleCounter = 0;
        }
    }

}
