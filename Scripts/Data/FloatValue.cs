using UnityEngine;

[CreateAssetMenu(menuName = "Data/FloatValue")]
public class FloatValue : GenericValue<float>
{
    public void Add(float f)
    {
        Value += f;
    }

    public void Subtract(float f)
    {
        Value -= f;
    }
}
