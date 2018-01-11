using System;
using UnityEngine;

namespace GameTemplate
{
    public class OnCollisionEnterReporter : MonoBehaviour
    {
        public event Action<Collision> OnCollisionEnterEvent;
        public event Action<Collision2D> OnCollisionEnter2DEvent;

        private void OnCollisionEnter(Collision collision)
        {
            if (OnCollisionEnterEvent != null)
                OnCollisionEnterEvent(collision);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (OnCollisionEnter2DEvent != null)
                OnCollisionEnter2DEvent(collision);
        }
    }
}
