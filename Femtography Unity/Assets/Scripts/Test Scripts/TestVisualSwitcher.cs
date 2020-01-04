using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVisualSwitcher : MonoBehaviour
{
    public GameObject objectOne, objectTwo;
    public float viewVectorAngle, moduloAngle;

    Renderer objectOneRenderer, objectTwoRenderer;
    MaterialPropertyBlock objectOnePropertyBlock, objectTwoPropertyBlock;
    Color objectOneColor, objectTwoColor;

    // Start is called before the first frame update
    void Start()
    {
        objectOneRenderer = objectOne.GetComponent<Renderer>();
        objectTwoRenderer = objectTwo.GetComponent<Renderer>();
        objectOneColor = objectOneRenderer.material.GetColor("_BaseColor");
        objectTwoColor = objectTwoRenderer.material.GetColor("_BaseColor");
        objectOnePropertyBlock = new MaterialPropertyBlock();
        objectTwoPropertyBlock = new MaterialPropertyBlock();

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 gazeDirection = Camera.main.transform.forward;
        Vector3 objectForward = transform.forward;
        viewVectorAngle = Vector3.Angle(gazeDirection, objectForward);

        if (Mathf.Floor(viewVectorAngle / moduloAngle) % 2 == 0)
        {
            ChangeOpacity(objectOne);
        } 
        
        else if (Mathf.Floor((viewVectorAngle) / moduloAngle) % 2 != 0)
        {
            ChangeOpacity(objectTwo);
        }
    }

    void ChangeOpacity(GameObject currentObject)
    {
        Color currentColor, otherColor;
        
        if (currentObject == objectOne)
        {
            currentColor = objectOneColor;
            otherColor = objectTwoColor;
        } else
        {
            currentColor = objectTwoColor;
            otherColor = objectOneColor;
        }

        float currentAngle = viewVectorAngle % moduloAngle;

        if (currentAngle < 11)
        {
            currentColor.a = ((currentAngle / 2) + 5) * .1f;
            otherColor.a = (10 - ((currentAngle / 2) + 5)) * .1f;
        } 

        float oppositeCurrentAngle = moduloAngle - currentAngle;

        if (oppositeCurrentAngle < 11)
        {
            currentColor.a = ((oppositeCurrentAngle / 2) + 5) * .1f;
            otherColor.a = (10 - ((oppositeCurrentAngle / 2) + 5)) * .1f;
        } 
        
        else if (currentAngle >= 11 && oppositeCurrentAngle >= 11)
        {
            currentColor.a = 1f;
            otherColor.a = 0f;
        }

        if (currentObject == objectOne)
        {
            objectOnePropertyBlock.SetColor("_BaseColor", currentColor);
            objectTwoPropertyBlock.SetColor("_BaseColor", otherColor);
        }
        else
        {
            objectTwoPropertyBlock.SetColor("_BaseColor", currentColor);
            objectOnePropertyBlock.SetColor("_BaseColor", otherColor);
        }

        objectOneRenderer.SetPropertyBlock(objectOnePropertyBlock);
        objectTwoRenderer.SetPropertyBlock(objectTwoPropertyBlock);
            
    }

}
