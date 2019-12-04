using System;
using UnityEngine;
using UnityEngine.Events;

namespace GameTemplate
{
    public class AnimationEventHandler : MonoBehaviour
    {
        [Serializable]
        public struct AnimationUnityEvent
        {
            public string Name;
            public UnityEvent Callback;
        }
        public event Action<string> AnimationEvent;
        public AnimationUnityEvent[] Callbacks;

        public void FireEvent(string id)
        {
            if (AnimationEvent != null)
                AnimationEvent(id);

            foreach (var callback in Callbacks)
                if (callback.Name == id)
                    callback.Callback.Invoke();
        }
    }
}
