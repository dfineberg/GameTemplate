using GameTemplate;
using UnityEngine;

public enum ValueListenerType { LessThan, LessThanOrEquals, GreaterThan, GreaterThanOrEquals, Equals, Not, Any }

public abstract class GenericValueListener<T, TU> : MonoBehaviour where T : GenericValue<TU>
{
    public T Value;
    public bool FireOnEnable = true;

    private void OnEnable()
    {
        if (FireOnEnable) Listener(Value.Value);
        Value.Subscribe(Listener);
    }

    private void OnDisable()
    {
        Value.Unsubscribe(Listener);
    }

    protected abstract void Listener(TU arg);
}
