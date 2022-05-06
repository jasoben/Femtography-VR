using UnityEngine;

namespace WaveMaker
{
    [RequireComponent(typeof(Camera))]
    public class FollowCam : MonoBehaviour
    {
        public Transform followedTransform;
        public float smoothness;

        Vector3 initialPosOffset;
        Quaternion initialRotOffset;
        Camera cam;

        void Start()
        {
            if (followedTransform == null)
            {
                enabled = false;
                Debug.LogWarning("Follow Cam component has no followed transform and it will be disabled");
            }

            cam = GetComponent<Camera>();
            initialPosOffset = transform.position - followedTransform.position;
            initialRotOffset = Quaternion.FromToRotation(followedTransform.forward, transform.forward);
        }

        void LateUpdate()
        {
            var destinationPos_ws = followedTransform.TransformPoint(initialPosOffset);
            var destinationRot_ws = followedTransform.rotation * initialRotOffset;

            var newPos = Vector3.Lerp(transform.position, destinationPos_ws, smoothness * Time.deltaTime);
            var newRot = Quaternion.Lerp(transform.rotation, destinationRot_ws, smoothness * Time.deltaTime);

            if (!DataCorrect(newPos, newRot) || !DataCorrect(destinationPos_ws, destinationRot_ws))
                return;
            else if ((destinationPos_ws - transform.position).magnitude > 20)
            {
                transform.position = destinationPos_ws;
                transform.rotation = destinationRot_ws;
            }
            else
            {
                transform.position = newPos;
                transform.rotation = newRot;
            }
        }

        public void CenterCamera()
        {
            var destinationPos_ws = followedTransform.TransformPoint(initialPosOffset);
            var destinationRot_ws = followedTransform.rotation * initialRotOffset;
            transform.position = destinationPos_ws;
            transform.rotation = destinationRot_ws;
        }

        // NOTE: Linux crash fix having NaN in values provoking "screen position out of view frustrum"
        bool DataCorrect(in Vector3 pos, in Quaternion rot)
        {
            return ! (float.IsNaN(pos.x) || float.IsNaN(pos.y) || float.IsNaN(pos.z)
                   || float.IsNaN(rot.x) || float.IsNaN(rot.y) || float.IsNaN(rot.z) || float.IsNaN(rot.w));
        }
    }
}