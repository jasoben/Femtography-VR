using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalSlider : PhysicalButton
{
    float zPos;
    Vector3 startPosRelative, endPosRelative, projectedMousePosition, litCylinderStartScale;
    bool mouseDown, mouseExit;
    GameObject startPositionObject, endPositionObject, litCylinder, litStartSphere, litEndSphere;
    public FloatReference sliderVariable;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        startPositionObject = transform.parent.Find("StartPoint").gameObject;
        endPositionObject = transform.parent.Find("EndPoint").gameObject;

        litCylinder = transform.parent.Find("LitSliderTrack").Find("Cylinder").gameObject;
        litStartSphere = transform.parent.Find("LitSliderTrack").Find("StartSphere").gameObject;
        litEndSphere = transform.parent.Find("LitSliderTrack").Find("EndSphere").gameObject;

        litCylinderStartScale = litCylinder.transform.localScale;

        SetLitColor();

        startPosRelative = transform.parent.InverseTransformPoint(startPositionObject.transform.position);
        endPosRelative = transform.parent.InverseTransformPoint(endPositionObject.transform.position);
    }

    void SetLitColor()
    {
        MaterialPropertyBlock litProperty = new MaterialPropertyBlock();
        litProperty.SetColor("Color_", GetComponent<PhysicalButton>().highlightButtonColor);
        litProperty.SetColor("GlowColor", GetComponent<PhysicalButton>().highlightButtonColor);
        litProperty.SetFloat("Alpha_", 1f);

        litCylinder.GetComponent<Renderer>().SetPropertyBlock(litProperty);
        litStartSphere.GetComponent<Renderer>().SetPropertyBlock(litProperty);
        litEndSphere.GetComponent<Renderer>().SetPropertyBlock(litProperty);
    }

    private new void OnMouseDown()
    {
        base.OnMouseDown();
        zPos = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mouseDown = true;
    }
    private new void OnMouseEnter()
    {
        base.OnMouseEnter();
        mouseExit = false;
    }
    private void OnMouseDrag()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, zPos));

        projectedMousePosition = Math3d.ProjectPointOnLineSegment(startPositionObject.transform.position,
            endPositionObject.transform.position, mousePosition);
        transform.position = projectedMousePosition;
        originalUITextOrImagePosition = transform.parent.InverseTransformPoint(projectedMousePosition); // leave the
                                                                                                        // slider where it is instead of returning it to it's zero position like a button (see parent class for 
                                                                                                        // more info on how buttons work)
        LightSliderTrack();
    }

    private void LightSliderTrack()
    {
        float sliderWidth = Vector3.Distance(startPositionObject.transform.position, endPositionObject.transform.position);
        float currentPositionWidth = Vector3.Distance(transform.position, startPositionObject.transform.position);
        float percentageWidth = currentPositionWidth / sliderWidth;
        Vector3 midPoint = (startPositionObject.transform.position + transform.position) / 2;

        if (percentageWidth > .01f)
        {
            litStartSphere.SetActive(true);
            litCylinder.SetActive(true);
            litCylinder.transform.position = midPoint;
            litCylinder.transform.localScale = new Vector3(litCylinderStartScale.x, 
                litCylinderStartScale.y * percentageWidth, litCylinderStartScale.z);
        }
        else if (percentageWidth < .01f)
        {
            litStartSphere.SetActive(false);
            litCylinder.SetActive(false);
        }
        if (percentageWidth > .95f)
        {
            litEndSphere.SetActive(true);
        }
        else if (percentageWidth < .95f)
        {
            litEndSphere.SetActive(false);
        }

        sliderVariable.Value = percentageWidth;
    }

    private new void OnMouseUp()
    {
        base.OnMouseUp();
        if (mouseExit == true)
            base.OnMouseExit();// since this is a slider we activate this since sometimes the mouse is 
                // outside the collider when released, thus "OnMouseExit()" won't fire (see boolean below)
        mouseDown = false;
    }
    private new void OnMouseExit()
    {
        mouseExit = true;
        if (!mouseDown) // since this is a slider we don't want the MouseExit animation to trigger if 
                // the user is still holding down the button, since users don't keep their cursor directly
                // over slider gizmos while dragging
            base.OnMouseExit();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
