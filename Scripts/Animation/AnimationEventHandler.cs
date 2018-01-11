using System;
using UnityEngine;

namespace GameTemplate
{
    public class AnimationEventHandler : MonoBehaviour
    {
        public event Action<string> AnimationEvent;

        public void FireEvent(string id)
        {
            if (AnimationEvent != null)
                AnimationEvent(id);
        }
    }
}
