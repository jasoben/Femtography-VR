using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVisualSwitcher : MonoBehaviour
{
    public GameObject objectOne, objectTwo;
    GameObject currentObject;
    public float viewVectorAngle, moduloAngle;

    bool switchObject, canISwitch;

    // Start is called before the first frame update
    void Start()
    {
        canISwitch = true;
        switchObject = true;
        currentObject = objectOne;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 gazeDirection = Camera.main.transform.forward;
        Vector3 objectForward = transform.forward;
        viewVectorAngle = Vector3.Angle(gazeDirection, objectForward);

        if (Mathf.Floor(viewVectorAngle) % moduloAngle == 0 && canISwitch)
        {
            canISwitch = false;
            SwitchObject();
        } else if (Mathf.Floor(viewVectorAngle) % moduloAngle != 0)
        {
            canISwitch = true;
        }
        
        if (Mathf.Floor(viewVectorAngle + 5) % moduloAngle == 0)
        {
            var block = new MaterialPropertyBlock();
            Color currentColor = currentObject.GetComponent<Renderer>().material.color;
            currentColor.a = .1f;
            block.SetColor("_BaseColor", currentColor);
            currentObject.GetComponent<Renderer>().SetPropertyBlock(block);
        }
    }

    void SwitchObject()
    {
        switchObject = !switchObject;

        currentObject.SetActive(false);

        switch (switchObject)
        {
            case true:
                currentObject = objectOne;
                break;
            case false:
                currentObject = objectTwo;
                break;
        }

        currentObject.SetActive(true);


    }
}
