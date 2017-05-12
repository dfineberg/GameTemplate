using System;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public event Action<string> AnimationEvent;

    public void FireEvent(string id)
    {
        if (AnimationEvent != null)
            AnimationEvent(id);
    }
}
