using System;
using UnityEngine;

namespace GameTemplate
{
    [CreateAssetMenu(menuName = "Events/Basic Event")]
    public class BasicEvent : ScriptableObject
    {
        private event Action Action;

        public int ListenerCount => Action != null ? Action.GetInvocationList().Length : 0;

        public void Subscribe(Action a)
        {
            Action += a;
        }

        public void Unsubscribe(Action a)
        {
            Action -= a;
        }

        public void Invoke()
        {
            Action?.Invoke();
        }
    }
}


