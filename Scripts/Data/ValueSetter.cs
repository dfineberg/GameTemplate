using GameTemplate;
using UnityEngine;

public class ValueSetter : MonoBehaviour
{
    public BoolValue BoolValue;
    public IntValue IntValue;
    public FloatValue FloatValue;
    public ReferenceCountedBoolValue ReferenceCountedBoolValue;

    public void SetBool(bool b) => BoolValue.Value = b;
    public void SetBoolWithInt(int i) => BoolValue.Value = i != 0; // animation events can't pass bool values
    public void SetInt(int i) => IntValue.Value = i;
    public void SetFloat(float f) => FloatValue.Value = f;
    public void SetReferenceCountedBoolValue(bool b, string key) => ReferenceCountedBoolValue.SetValue(b, key);
}
