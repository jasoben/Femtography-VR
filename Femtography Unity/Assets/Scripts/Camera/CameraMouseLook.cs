using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMouseLook : MonoBehaviour
{
    Vector2 rotation;
    [SerializeField] float sensitivity;
    private bool mouseLookActive;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (!mouseLookActive)
            return;

        Vector2 lookValue = context.ReadValue<Vector2>();

        rotation.y -= lookValue.x * sensitivity;
        rotation.x += lookValue.y * sensitivity;
        transform.rotation = Quaternion.Euler(rotation);
    }

    public void ActivateMouseLook(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            mouseLookActive = true;
        }
            
        else if (context.canceled)
        {
            mouseLookActive = false;
        }
    }

}
