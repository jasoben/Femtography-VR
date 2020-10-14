using UnityEngine;

namespace WaveMaker
{
    [RequireComponent(typeof(Rigidbody))]
    public class WaveMaker_SampleScene_Ship_Mover : MonoBehaviour
    {
        Rigidbody rb;

        public float frontThrust = 1f;
        public float backThrust = 0.5f;
        public float sideTorque = 1f;
        public float maxVelocity = 6f;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.UpArrow))
                rb.AddForce(transform.localToWorldMatrix.MultiplyVector(Vector3.forward * frontThrust), ForceMode.Force);
            else if (Input.GetKey(KeyCode.DownArrow))
                rb.AddForce(transform.localToWorldMatrix.MultiplyVector(Vector3.back * backThrust), ForceMode.Force);

            if (Input.GetKey(KeyCode.RightArrow))
                rb.AddTorque(transform.localToWorldMatrix.MultiplyVector(Vector3.up * sideTorque), ForceMode.Force);
            else if (Input.GetKey(KeyCode.LeftArrow))
                rb.AddTorque(transform.localToWorldMatrix.MultiplyVector(-Vector3.up * sideTorque), ForceMode.Force);

            //Block speed
            if (rb.angularVelocity.magnitude > maxVelocity)
                rb.angularVelocity = rb.angularVelocity.normalized * maxVelocity;

            if (rb.velocity.magnitude > maxVelocity)
                rb.velocity = rb.velocity.normalized* maxVelocity;

        }
    }
}
