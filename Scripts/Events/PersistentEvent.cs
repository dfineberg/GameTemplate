using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Basic Event")]
public class PersistentEvent : ScriptableObject
{
    private event Action Action;

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


