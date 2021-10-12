using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestRotateWithXRController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RotateCube(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<Vector2>());
        float rotationAmount = context.ReadValue<Vector2>().y;
        transform.Rotate(rotationAmount, 0, 0);
    }


}
