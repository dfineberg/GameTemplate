using GameTemplate;
using UnityEngine;

public class BoolValueBetweenListener : MonoBehaviour
{
    public BoolValue FirstValue;
    public BoolValue SecondValue;
    public bool FireOnEnable = true;
    public UnityEventBool Callback;

    private void OnEnable()
    {
        if (FireOnEnable) Evaluate(false);
        FirstValue.Subscribe(Evaluate);
        SecondValue.Subscribe(Evaluate);
    }

    private void OnDisable()
    {
        FirstValue.Unsubscribe(Evaluate);
        SecondValue.Unsubscribe(Evaluate);
    }

    private void Evaluate(bool b)
    {
        var param = FirstValue.Value && !SecondValue.Value;
        Callback.Invoke(param);
    }
}