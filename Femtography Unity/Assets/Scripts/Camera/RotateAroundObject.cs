using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotateAroundObject : MonoBehaviour
{
    public GameObject centerObject, XRRig;
    public float rotateAmount;
    float adjustedRotateAmount;

    // Start is called before the first frame update
    void Start()
    {
        adjustedRotateAmount = rotateAmount;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKey(KeyCode.Q)) {
        //    transform.RotateAround(centerObject.transform.position, Vector3.up, adjustedRotateAmount);
        //    IncreaseRotateAmount();
        //} else if (Input.GetKey(KeyCode.E))
        //{
        //    transform.RotateAround(centerObject.transform.position, Vector3.up, -adjustedRotateAmount);
        //    IncreaseRotateAmount();
        //}

        //if (Input.GetKey(KeyCode.W))
        //{
        //    transform.RotateAround(centerObject.transform.position, transform.right, adjustedRotateAmount);
        //    IncreaseRotateAmount();
        //} else if (Input.GetKey(KeyCode.S))
        //{
        //    transform.RotateAround(centerObject.transform.position, transform.right, -adjustedRotateAmount);
        //    IncreaseRotateAmount();
        //}

        //if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        //    adjustedRotateAmount = rotateAmount;

        transform.LookAt(centerObject.transform.position);

    }

    void IncreaseRotateAmount()
    {
        adjustedRotateAmount += .001f;
    }

    public void RotateAroundCentralObject(InputAction.CallbackContext callbackContext)
    {
        adjustedRotateAmount = callbackContext.ReadValue<Vector2>().x;
        XRRig.transform.RotateAround(centerObject.transform.position, Vector3.up, adjustedRotateAmount);
    }
}
