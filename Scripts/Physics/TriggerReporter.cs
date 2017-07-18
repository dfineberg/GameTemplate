using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ColliderEvent : UnityEvent<Collider>
{
}

public class TriggerReporter : MonoBehaviour
{
    public LayerMask TriggerLayers = Physics.AllLayers;
    [Tooltip("Only fire events for the first object to enter and the last object to exit")]
    public bool OnlyFirstAndLast;
    public ColliderEvent OnTriggerEnterEvent;
    public ColliderEvent OnTriggerExitEvent;

    private readonly List<Collider> _list = new List<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        if (!TriggerLayers.ContainsLayer(other.gameObject.layer)) 
            return;
        
        _list.Add(other);
        
        if(OnlyFirstAndLast && _list.Count == 1 || !OnlyFirstAndLast)
            OnTriggerEnterEvent.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!TriggerLayers.ContainsLayer(other.gameObject.layer)) 
            return;
        
        _list.Remove(other);

        if (OnlyFirstAndLast && _list.Count == 0 || !OnlyFirstAndLast)
            OnTriggerExitEvent.Invoke(other);
    }

    private void OnDisable()
    {
        if (_list.Count == 0)
            return;

        var col = _list[0];
        _list.Clear();
        OnTriggerExitEvent.Invoke(col);
    }
}