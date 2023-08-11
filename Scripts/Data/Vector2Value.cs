using GameTemplate;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Vector2Value")]
public class Vector2Value : GenericValue<Vector2>
{
    protected override bool Equals(Vector2 other)
    {
        return Value == other;
    }
}
