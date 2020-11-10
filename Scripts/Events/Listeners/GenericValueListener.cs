using GameTemplate;
using UnityEngine;

public enum ValueListenerType { LessThan, LessThanOrEquals, GreaterThan, GreaterThanOrEquals, Equals, Not, Any }

public abstract class GenericValueListener<T, TU> : MonoBehaviour where T : GenericValue<TU>
{
    public T Value;
    public bool FireOnEnable = true;

    private void OnEnable()
    {
        if (Value == null)
        {
            Debug.LogError(gameObject.name + " doesn't have listener value set!");
            return;
        }

        if (FireOnEnable) Listener(Value.Value);
        Value.Subscribe(Listener);
    }

    private void OnDisable()
    {
        if (Value == null) return;

        Value.Unsubscribe(Listener);
    }

    protected abstract void Listener(TU arg);
}
