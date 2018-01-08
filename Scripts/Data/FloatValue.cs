using UnityEngine;

[CreateAssetMenu(menuName = "Data/FloatValue")]
public class FloatValue : ScriptableObject
{
    public float Value;

    public void Add(float f)
    {
        Value += f;
    }

    public void Subtract(float f)
    {
        Value -= f;
    }

    public void Set(float f)
    {
        Value = f;
    }
}
