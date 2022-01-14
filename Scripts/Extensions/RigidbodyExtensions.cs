using UnityEngine;

namespace GameTemplate
{
    public static class RigidbodyExtensions
    {
        public static Vector3 Forward(this Rigidbody rb)
        {
            return rb.rotation * Vector3.forward;
        }
    }
}