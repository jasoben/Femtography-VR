using UnityEngine;

namespace WaveMaker
{
    static class WaveMakerUtils
    {
        static float epsilon = 0.001f;

        /// <summary>
        /// Transform Bounds from worldSpace to localSpace given a transform matrix
        /// </summary>
        public static Bounds TransformBounds(Bounds inBounds, Matrix4x4 matrix)
        {
            var xa = matrix.GetColumn(0) * inBounds.min.x;
            var xb = matrix.GetColumn(0) * inBounds.max.x;

            var ya = matrix.GetColumn(1) * inBounds.min.y;
            var yb = matrix.GetColumn(1) * inBounds.max.y;

            var za = matrix.GetColumn(2) * inBounds.min.z;
            var zb = matrix.GetColumn(2) * inBounds.max.z;

            Bounds result = new Bounds();
            Vector3 pos = matrix.GetColumn(3);
            result.SetMinMax(Vector3.Min(xa, xb) + Vector3.Min(ya, yb) + Vector3.Min(za, zb) + pos,
                             Vector3.Max(xa, xb) + Vector3.Max(ya, yb) + Vector3.Max(za, zb) + pos);
            return result;
        }

        public static bool IsPointInsideCollider(Collider collider, Vector3 point)
        {
            Vector3 pointInCollider= collider.ClosestPoint(point);
            pointInCollider.x -= point.x;
            pointInCollider.y -= point.y;
            pointInCollider.z -= point.z;
            return pointInCollider.x * pointInCollider.x + pointInCollider.y * pointInCollider.y + pointInCollider.z * pointInCollider.z < epsilon;
        }

        /// <summary>
        /// Given a point in a given space and a center of rotation in the given space, returns the velocity at that point
        /// </summary>
        public static Vector3 VelocityAtPoint(Vector3 point, Vector3 center, Vector3 angularVelocity, Vector3 linearVelocity)
        {
            return Vector3.Cross(angularVelocity, point - center) + linearVelocity;
        }

        /// <summary>
        /// Given an old and new rotation quaternions, return the angular velocity of the given object
        /// </summary>
        /// <param name="oldQuat">rotation before (normalized)</param>
        /// <param name="newQuat">rotation after a fixedDeltaTime (normalized)</param>
        public static Vector3 GetAngularVelocity(Quaternion oldQuat, Quaternion newQuat)
        {
            float scaledTime = 2 / Time.fixedDeltaTime;
            oldQuat.x = -oldQuat.x;
            oldQuat.y = -oldQuat.y;
            oldQuat.z = -oldQuat.z;
            oldQuat = newQuat * oldQuat;
            return new Vector3(oldQuat.x, oldQuat.y, oldQuat.z) * scaledTime;
        }
    }

}
