using UnityEngine;

public class BoolValueListener : MonoBehaviour
{
    public BoolValue BoolValue;
    public UnityEventBool Callback;

    private void OnEnable()
    {
        Callback.Invoke(BoolValue.Value);
        BoolValue.Subscribe(Listener);
    }

    private void OnDisable()
    {
        BoolValue.Unsubscribe(Listener);
    }

    private void Listener(bool b)
    {
        Callback.Invoke(b);
    }
}
