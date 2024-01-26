using System;
using UnityEngine;

namespace GameTemplate
{
    public class OnCollisionEnterReporter : MonoBehaviour
    {
#if PHYSICS3D_ENABLED
        public event Action<Collision> OnCollisionEnterEvent;
#endif
#if PHYSICS2D_ENABLED
        public event Action<Collision2D> OnCollisionEnter2DEvent;
#endif

#if PHYSICS3D_ENABLED
        private void OnCollisionEnter(Collision collision)
        {
            if (OnCollisionEnterEvent != null)
                OnCollisionEnterEvent(collision);
        }
#endif

#if PHYSICS2D_ENABLED
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (OnCollisionEnter2DEvent != null)
                OnCollisionEnter2DEvent(collision);
        }
#endif
    }
}
