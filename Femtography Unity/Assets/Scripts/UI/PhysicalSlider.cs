using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalSlider : PhysicalButton
{
    public float sliderDepth;
    float zPos;
    Vector3 startPosition, endPosition;
    bool mouseDown;
    GameObject startPositionObject, endPositionObject;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        startPositionObject = transform.parent.Find("StartPoint").gameObject;
        endPositionObject = transform.parent.Find("EndPoint").gameObject;
        //startPosition = transform.parent.InverseTransformPoint(startPositionObject.transform.position);
        //endPosition = transform.parent.InverseTransformPoint(endPositionObject.transform.position);
        Debug.Log(transform.parent.name);
        Debug.Log(startPositionObject.name);
        Debug.Log(transform.parent.position);
        Debug.Log(startPositionObject.transform.position);
        Debug.Log(startPosition);
    }

    private new void OnMouseDown()
    {
        base.OnMouseDown();
        zPos = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mouseDown = true;
    }
    private void OnMouseDrag()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, zPos));
        Vector3 relativePos = transform.parent.InverseTransformPoint(transform.position);// we need the relative position
                                                                                  // to calculate the bounds of the slider
        Debug.Log($"relative x {relativePos.x} and startPosition.x {startPosition.x}");

        if (relativePos.x > startPosition.x && relativePos.x < endPosition.x) // if it is within bounds of
                                                                              // the slider 
        {
            Vector3 projectedVector = Math3d.ProjectPointOnLineSegment(startPositionObject.transform.position,
                endPositionObject.transform.position, mousePosition);
            transform.position = projectedVector;
        }
    }
    private new void OnMouseUp()
    {
        base.OnMouseUp();
        mouseDown = false;
    }
    private new void OnMouseExit()
    {
        if (!mouseDown)
            base.OnMouseExit();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
