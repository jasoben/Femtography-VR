using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicalSlider : PhysicalButton
{
    float sliderPosRelativeToWidth;
    Vector3 projectedPosition, litCylinderStartScale;
    GameObject startPositionObject, endPositionObject, litCylinder, litStartSphere, litEndSphere;
    public FloatReference sliderVariable;
    public UnityEvent newValueSet;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        sliderPosRelativeToWidth = sliderVariable.Value;

        startPositionObject = transform.parent.Find("StartPoint").gameObject;
        endPositionObject = transform.parent.Find("EndPoint").gameObject;

        litCylinder = transform.parent.Find("LitSliderTrack").Find("Cylinder").gameObject;
        litStartSphere = transform.parent.Find("LitSliderTrack").Find("StartSphere").gameObject;
        litEndSphere = transform.parent.Find("LitSliderTrack").Find("EndSphere").gameObject;

        litCylinderStartScale = litCylinder.transform.localScale;

        SetLitColor();

        CalculateBlobPositionAndLightTrack();
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

    public void UpdateSliderPosition(Vector3 trackerPosition)
    {
        projectedPosition = Math3d.ProjectPointOnLineSegment(startPositionObject.transform.position,
            endPositionObject.transform.position, trackerPosition);
        transform.position = projectedPosition;
        originalUITextOrImagePosition = transform.parent.InverseTransformPoint(projectedPosition); // leave the
                                                                                                        // slider where it is instead of returning it to it's zero position like a button (see parent class for 
                                                                                                        // more info on how buttons work)
        CalculateBlobPositionAndLightTrack();
    }

    private void CalculateBlobPositionAndLightTrack()
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

        percentageWidth = Mathf.Clamp01(percentageWidth);

        sliderPosRelativeToWidth = percentageWidth;
        sliderVariable.Value = percentageWidth;
    }

    public void SetNewValue()
    {
        newValueSet.Invoke();
    }
    // Update is called once per frame
    void Update()
    {
        if (sliderVariable.Value != sliderPosRelativeToWidth)
        {
            float distanceStartToEnd = transform.parent.InverseTransformPoint(endPositionObject.transform.position).x
                - transform.parent.InverseTransformPoint(startPositionObject.transform.position).x;
            float sliderBlobPositionX = transform.parent.InverseTransformPoint(startPositionObject.transform.position).x
                + (distanceStartToEnd * sliderVariable.Value);

            transform.position = transform.parent.TransformPoint(sliderBlobPositionX, 0, 0);

            CalculateBlobPositionAndLightTrack();
        } 
        
    }
}
