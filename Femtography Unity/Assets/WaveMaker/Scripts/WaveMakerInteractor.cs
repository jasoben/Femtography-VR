using System;
using UnityEngine;

namespace WaveMaker
{
    public class WaveMakerInteractor : MonoBehaviour
    {
        public Vector3 LinearVelocity { get; private set; }
        public Vector3 AngularVelocity { get; private set; }
        public Vector3 CenterOfMass { get { return usesRigidBody ? transform.TransformPoint(rb.centerOfMass) : transform.position; } }

        [Tooltip("This will make velocity values change softer, making the response of the WaveMaker object softer too. Disable for efficiency gain")]
        public bool speedDampening = false;

        [Tooltip("Higher value means slower velocity change")]
        [Range(0, 1)]
        public float speedDampValue = 0f;

        [Tooltip("Shows the linear and angular velocies in the scene view during play")]
        public bool showVelocities = false;

        Vector3 _lastPosition;
        Quaternion _lastRotation;
        Rigidbody rb;

        /// <summary>
        /// Only Non Kinematic rigid bodies allow to calculate some values needed. Otherwise we do it by hand.
        /// </summary>
        bool usesRigidBody = false;

        void Awake()
        {
            _lastPosition = transform.position;
            _lastRotation = transform.rotation;
            UpdateRigidBodyStatus();

            var meshColliders = GetComponents<MeshCollider>();
            foreach (var mesh in meshColliders)
            {
                if (!mesh.convex)
                {
                    mesh.enabled = false;
                    Debug.LogError("WaveMaker - (" + gameObject.name + ") has a mesh collider that is not convex. Mesh colliders are slow in contact, but it will slow down any WaveMaker surface it touches even more. Disabled!");
                }
            }
        }

        void FixedUpdate()
        {
            if (showVelocities)
            {
                UpdateVelocities();
                Debug.DrawRay(transform.position, LinearVelocity, Color.red);
                Debug.DrawRay(transform.position, AngularVelocity, Color.blue);
            }
        }

        public void UpdateVelocities()
        {
            Vector3 oldLinearVelocity = LinearVelocity;

            if (usesRigidBody)
            {
                LinearVelocity = rb.velocity;
                AngularVelocity = rb.angularVelocity;
            }
            else
            {
                LinearVelocity = (transform.position - _lastPosition) / Time.fixedDeltaTime;
                _lastPosition = transform.position;

                AngularVelocity = WaveMakerUtils.GetAngularVelocity(_lastRotation, transform.rotation);
                _lastRotation = transform.rotation;
            }

            if (speedDampening)
                LinearVelocity = Vector3.Lerp(oldLinearVelocity, LinearVelocity, 1 - speedDampValue);
        }

        /// <summary>
        /// Call this if you make changes to the interactor after execution, attach a rigidbody or change the kinematic status of it.
        /// </summary>
        public void UpdateRigidBodyStatus()
        {
            rb = GetComponent<Rigidbody>();
            usesRigidBody = rb != null && !rb.isKinematic;
        }
    }
}
