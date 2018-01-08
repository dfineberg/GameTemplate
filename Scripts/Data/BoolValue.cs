using UnityEngine;

[CreateAssetMenu(menuName = "Data/BoolValue")]
public class BoolValue : ScriptableObject
{
    public bool Value;

    public void Set(bool b)
    {
        Value = b;
    }
}
