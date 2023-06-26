using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMouseLook : MonoBehaviour
{
    Vector2 rotation;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(rotation);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 lookValue = context.ReadValue<Vector2>();

        Debug.Log(lookValue);

        rotation.y += lookValue.x;
        rotation.x += lookValue.y;


    }

}
