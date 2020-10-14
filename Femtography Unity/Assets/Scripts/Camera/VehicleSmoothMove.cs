using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSmoothMove : MonoBehaviour
{
    Vector3 vehicleMoveDirection, vehicleRotation;
    public float maxSpeed;
    bool slowDownX, slowDownY, slowDownZ;
    public GlobalBool flightModeEnabled;
    // Start is called before the first frame update
    void Start()
    {
        flightModeEnabled.boolValue = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (flightModeEnabled.boolValue)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                slowDownX = false;
                vehicleMoveDirection.x += -.01f;
                if (Mathf.Abs(vehicleMoveDirection.x) > maxSpeed)
                {
                    vehicleMoveDirection.x = -maxSpeed;
                }
            }
            if (Input.GetKeyUp(KeyCode.Q))
            {
                slowDownX = true;
            }

            if (Input.GetKey(KeyCode.E))
            {
                slowDownX = false;
                vehicleMoveDirection.x += .01f;
                if (Mathf.Abs(vehicleMoveDirection.x) > maxSpeed)
                {
                    vehicleMoveDirection.x = maxSpeed;
                }
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                slowDownX = true;
            }

            if (slowDownX)
            {
                if (vehicleMoveDirection.x < -.1)
                    vehicleMoveDirection.x += .02f;
                else if (vehicleMoveDirection.x > .1)
                    vehicleMoveDirection.x -= .02f;
                else if (vehicleMoveDirection.x > -.1 && vehicleMoveDirection.x < .1)
                    vehicleMoveDirection.x = 0;
            }

            if (Input.GetKey(KeyCode.S))
            {
                slowDownZ = false;
                vehicleMoveDirection.z += -.01f;
                if (Mathf.Abs(vehicleMoveDirection.z) > maxSpeed)
                {
                    vehicleMoveDirection.z = -maxSpeed;
                }
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                slowDownZ = true;
            }

            if (Input.GetKey(KeyCode.W))
            {
                slowDownZ = false;
                vehicleMoveDirection.z += .01f;
                if (Mathf.Abs(vehicleMoveDirection.z) > maxSpeed)
                {
                    vehicleMoveDirection.z = maxSpeed;
                }
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                slowDownZ = true;
            }

            if (slowDownZ)
            {
                if (vehicleMoveDirection.z < -.1)
                    vehicleMoveDirection.z += .02f;
                else if (vehicleMoveDirection.z > .1)
                    vehicleMoveDirection.z -= .02f;
                else if (vehicleMoveDirection.z > -.1 && vehicleMoveDirection.z < .1)
                    vehicleMoveDirection.z = 0;
            }

            if (Input.GetKey(KeyCode.Z))
            {
                slowDownY = false;
                vehicleMoveDirection.y += -.01f;
                if (Mathf.Abs(vehicleMoveDirection.y) > maxSpeed)
                {
                    vehicleMoveDirection.y = -maxSpeed;
                }
            }
            if (Input.GetKeyUp(KeyCode.Z))
            {
                slowDownY = true;
            }

            if (Input.GetKey(KeyCode.X))
            {
                slowDownY = false;
                vehicleMoveDirection.y += .01f;
                if (Mathf.Abs(vehicleMoveDirection.y) > maxSpeed)
                {
                    vehicleMoveDirection.y = maxSpeed;
                }
            }
            if (Input.GetKeyUp(KeyCode.X))
            {
                slowDownY = true;
            }

            if (Input.GetKey(KeyCode.D))
            {
                slowDownY = false;
                vehicleRotation.y += .01f;
                if (Mathf.Abs(vehicleRotation.y) > maxSpeed)
                {
                    vehicleRotation.y = maxSpeed;
                }
            }

            if (Input.GetKeyUp(KeyCode.D))
            {
                slowDownY = true;
            }
            if (Input.GetKey(KeyCode.A))
            {
                slowDownY = false;
                vehicleRotation.y -= .01f;
                if (Mathf.Abs(vehicleRotation.y) > maxSpeed)
                {
                    vehicleRotation.y = maxSpeed;
                }
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                slowDownY = true;
            }
            if (slowDownY)
            {
                if (vehicleMoveDirection.y < -.1)
                    vehicleMoveDirection.y += .02f;
                else if (vehicleMoveDirection.y > .1)
                    vehicleMoveDirection.y -= .02f;
                else if (vehicleMoveDirection.y > -.1 && vehicleMoveDirection.y < .1)
                    vehicleMoveDirection.y = 0;

                if (vehicleRotation.y < -.1)
                    vehicleRotation.y += .02f;
                else if (vehicleRotation.y > .1)
                    vehicleRotation.y -= .02f;
                else if (vehicleRotation.y > -.1 && vehicleRotation.y < .1)
                    vehicleRotation.y = 0;
            }

            if (Input.GetKeyUp(KeyCode.M))
            {
                //GetComponent<MouseLook>().enabled = !GetComponent<MouseLook>().enabled;
            }
            gameObject.transform.Translate(vehicleMoveDirection);
            gameObject.transform.Rotate(vehicleRotation);
        }

    }
}
