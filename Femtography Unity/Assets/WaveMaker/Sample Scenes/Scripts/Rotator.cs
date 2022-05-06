using UnityEngine;

namespace WaveMaker
{
    public class Rotator : MonoBehaviour
    {
        public float rotationSpeed = 200;

        void Update()
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                Vector3 rotationSpeed_ws = transform.TransformDirection(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
                transform.Rotate(rotationSpeed_ws, Space.World);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                Vector3 rotationSpeed_ws = transform.TransformDirection(new Vector3(0, -rotationSpeed * Time.deltaTime, 0));
                transform.Rotate(rotationSpeed_ws, Space.World);
            }
            else
            {
                Vector3 rotationSpeed_ws = transform.TransformDirection(new Vector3(0, rotationSpeed * 0.3f * Time.deltaTime, 0));
                transform.Rotate(rotationSpeed_ws, Space.World);
            }
        }
    }
}