using System;
using UnityEngine;
using UnityEngine.Events;

public class BoolEventListener : MonoBehaviour
{
    [Serializable]
    public class UnityBoolEvent : UnityEvent<bool>{}
    
    public BoolEvent BoolEvent;
    public UnityBoolEvent Callback;

    private void OnEnable()
    {
        BoolEvent?.Subscribe(Listener);
    }

    private void OnDisable()
    {
        BoolEvent?.Unsubscribe(Listener);
    }

    private void Listener(bool b)
    {
        Callback.Invoke(b);
    }
}
