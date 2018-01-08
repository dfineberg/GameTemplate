using System;
using UnityEngine;

public class GenericPersistentEvent<T> : ScriptableObject
{
    public T DefaultValue;
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

    public void InvokeWithDefault()
    {
        Invoke(DefaultValue);
    }
}
