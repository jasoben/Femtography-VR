using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVisualSwitcher : MonoBehaviour
{
    public GameObject objectOne, objectTwo;
    public float viewVectorAngle, moduloAngle;

    // Start is called before the first frame update
    void Start()
    {
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
        } else if (Mathf.Floor(viewVectorAngle / moduloAngle) % 2 != 0)
        {
            ChangeOpacity(objectTwo);
        }
    }

    void ChangeOpacity(GameObject currentObject)
    {
        var block = new MaterialPropertyBlock();
        Color currentColor = currentObject.GetComponent<Renderer>().material.color;

        for (int i = 0; i < 11; i++)
        {
            if (Mathf.Floor(viewVectorAngle + i) % moduloAngle == 0)
            {
                currentColor.a = i * .1f;
            }
        }
            
        for (int i = 10; i > -1; i--)
        {
            if (Mathf.Floor(viewVectorAngle - i) % moduloAngle == 0)
            {
                currentColor.a = i * .1f;
            }
        }

        block.SetColor("_BaseColor", currentColor);
        currentObject.GetComponent<Renderer>().SetPropertyBlock(block);
            
    }

}
