using UnityEngine.Events;

public class BasicEventListener : EventListener
{
    public UnityEvent CallbackEvent;
    
    protected override void Callback()
    {
        CallbackEvent.Invoke();
    }
}
