using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ColliderEvent : UnityEvent<Collider>
{
}

public class TriggerReporter : MonoBehaviour
{
    public LayerMask TriggerLayers = Physics.AllLayers;
    public ColliderEvent OnTriggerEnterEvent;
    public ColliderEvent OnTriggerExitEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (TriggerLayers.ContainsLayer(other.gameObject.layer))
            OnTriggerEnterEvent.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (TriggerLayers.ContainsLayer(other.gameObject.layer))
            OnTriggerExitEvent.Invoke(other);
    }
}