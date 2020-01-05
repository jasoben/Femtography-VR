using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVisualSwitcher : MonoBehaviour
{
    public GameObject objectOne, objectTwo;
    public float viewVectorAngle, moduloAngle;
    public string propertyToChange;

    Renderer objectOneRenderer, objectTwoRenderer;
    MaterialPropertyBlock objectOnePropertyBlock, objectTwoPropertyBlock;

    // Start is called before the first frame update
    void Start()
    {
        objectOneRenderer = objectOne.GetComponent<Renderer>();
        objectTwoRenderer = objectTwo.GetComponent<Renderer>();
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
        float currentOpacity = 1, otherOpacity = 1;
        
        float currentAngle = viewVectorAngle % moduloAngle;

        if (currentAngle < 11)
        {
            currentOpacity = ((currentAngle / 2) + 5) * .1f;
            otherOpacity = (10 - ((currentAngle / 2) + 5)) * .1f;
        } 

        float oppositeCurrentAngle = moduloAngle - currentAngle;

        if (oppositeCurrentAngle < 11)
        {
            currentOpacity = ((oppositeCurrentAngle / 2) + 5) * .1f;
            otherOpacity = (10 - ((oppositeCurrentAngle / 2) + 5)) * .1f;
        } 
        
        else if (currentAngle >= 11 && oppositeCurrentAngle >= 11)
        {
            currentOpacity = 1f;
            otherOpacity = 0f;
        }

        if (currentObject == objectOne)
        {
            objectOnePropertyBlock.SetFloat(propertyToChange, currentOpacity);
            objectTwoPropertyBlock.SetFloat(propertyToChange, otherOpacity);
        }
        else
        {
            objectTwoPropertyBlock.SetFloat(propertyToChange, currentOpacity);
            objectOnePropertyBlock.SetFloat(propertyToChange, otherOpacity);
        }

        objectOneRenderer.SetPropertyBlock(objectOnePropertyBlock);
        objectTwoRenderer.SetPropertyBlock(objectTwoPropertyBlock);
            
    }

}
