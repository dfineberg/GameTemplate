using GameTemplate;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Vector3Value")]
public class Vector3Value : GenericValue<Vector3>
{
    protected override bool Equals(Vector3 other)
    {
        return Value == other;
    }
}
