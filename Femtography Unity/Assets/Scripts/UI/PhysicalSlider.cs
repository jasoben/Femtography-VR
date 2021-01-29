using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalSlider : PhysicalButton
{
    public float sliderDepth;
    float zPos;
    Vector3 startPosition, endPosition;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        startPosition = transform.parent.Find("StartPoint").position;
        endPosition = transform.parent.Find("EndPoint").position;
    }

    private new void OnMouseDown()
    {
        base.OnMouseDown();
        zPos = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
    }
    private void OnMouseDrag()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, zPos));
        if (transform.position.x > startPosition.x && transform.position.x < endPosition.x) // if it is within bounds of
            // the slider 
            transform.position = new Vector3(mousePosition.x, transform.position.y, transform.position.z);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
