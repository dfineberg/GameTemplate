using GameTemplate;
using UnityEngine;

public enum ValueListenerType { LessThan, LessThanOrEquals, GreaterThan, GreaterThanOrEquals, Equals, Not, Any }

public abstract class GenericValueListener<T, TU> : MonoBehaviour where T : GenericValue<TU>
{
    public T Value;

    private void OnEnable()
    {
        Listener(Value.Value);
        Value.Subscribe(Listener);
    }

    private void OnDisable()
    {
        Value.Unsubscribe(Listener);
    }

    protected abstract void Listener(TU arg);
}
