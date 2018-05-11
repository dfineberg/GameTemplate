using GameTemplate;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/GameObjectValue")]
public class GameObjectValue : GenericValue<GameObject> {
    protected override bool Equals(GameObject other)
    {
        return Value == other;
    }
}
