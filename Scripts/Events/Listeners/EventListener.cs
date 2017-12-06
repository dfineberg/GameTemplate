using UnityEngine;

public abstract class EventListener : MonoBehaviour
{
    public PersistentEvent Event;
    
    protected virtual void OnEnable()
    {
        Event?.Subscribe(Callback);
    }

    protected virtual void OnDisable()
    {
        Event?.Unsubscribe(Callback);
    }

    protected abstract void Callback();
}
