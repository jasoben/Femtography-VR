using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothMove : MonoBehaviour
{
    Vector3 cameraMoveDirection;
    public float maxSpeed;
    bool slowDownX, slowDownY, slowDownZ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            slowDownX = false;
            cameraMoveDirection.x += -.01f;
            if (Mathf.Abs(cameraMoveDirection.x) > maxSpeed)
            {
                cameraMoveDirection.x = -maxSpeed;
            }
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            slowDownX = true;
        }

        if (Input.GetKey(KeyCode.D))
        {
            slowDownX = false;
            cameraMoveDirection.x += .01f;
            if (Mathf.Abs(cameraMoveDirection.x) > maxSpeed)
            {
                cameraMoveDirection.x = maxSpeed;
            }
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            slowDownX = true;
        }

        if (slowDownX)
        {
            if (cameraMoveDirection.x < -.1)
                cameraMoveDirection.x += .02f;
            else if (cameraMoveDirection.x > .1)
                cameraMoveDirection.x -= .02f;
            else if (cameraMoveDirection.x > -.1 && cameraMoveDirection.x < .1)
                cameraMoveDirection.x = 0;
        }

        if (Input.GetKey(KeyCode.S))
        {
            slowDownZ = false;
            cameraMoveDirection.z += -.01f;
            if (Mathf.Abs(cameraMoveDirection.z) > maxSpeed)
            {
                cameraMoveDirection.z = -maxSpeed;
            }
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            slowDownZ = true;
        }

        if (Input.GetKey(KeyCode.W))
        {
            slowDownZ = false;
            cameraMoveDirection.z += .01f;
            if (Mathf.Abs(cameraMoveDirection.z) > maxSpeed)
            {
                cameraMoveDirection.z = maxSpeed;
            }
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            slowDownZ = true;
        }

        if (slowDownZ)
        {
            if (cameraMoveDirection.z < -.1)
                cameraMoveDirection.z += .02f;
            else if (cameraMoveDirection.z > .1)
                cameraMoveDirection.z -= .02f;
            else if (cameraMoveDirection.z > -.1 && cameraMoveDirection.z < .1)
                cameraMoveDirection.z = 0;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            slowDownY = false;
            cameraMoveDirection.y += -.01f;
            if (Mathf.Abs(cameraMoveDirection.y) > maxSpeed)
            {
                cameraMoveDirection.y = -maxSpeed;
            }
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            slowDownY = true;
        }

        if (Input.GetKey(KeyCode.E))
        {
            slowDownY = false;
            cameraMoveDirection.y += .01f;
            if (Mathf.Abs(cameraMoveDirection.y) > maxSpeed)
            {
                cameraMoveDirection.y = maxSpeed;
            }
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            slowDownY = true;
        }

        if (slowDownY)
        {
            if (cameraMoveDirection.y < -.1)
                cameraMoveDirection.y += .02f;
            else if (cameraMoveDirection.y > .1)
                cameraMoveDirection.y -= .02f;
            else if (cameraMoveDirection.y > -.1 && cameraMoveDirection.y < .1)
                cameraMoveDirection.y = 0;
        }

        if (Input.GetKeyUp(KeyCode.M))
        {
            //GetComponent<MouseLook>().enabled = !GetComponent<MouseLook>().enabled;
        }
        gameObject.transform.Translate(cameraMoveDirection);
    }

}
