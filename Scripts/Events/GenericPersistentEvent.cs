using System;
using UnityEngine;

public class GenericPersistentEvent<T> : ScriptableObject
{
    private event Action<T> Action;

    public void Subscribe(Action<T> a)
    {
        Action += a;
    }

    public void Unsubscribe(Action<T> a)
    {
        Action -= a;
    }

    public void Invoke(T arg)
    {
        Action?.Invoke(arg);
    }
}
