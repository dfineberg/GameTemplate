using UnityEngine;

[CreateAssetMenu(menuName = "Data/BoolValue")]
public class BoolValue : GenericValue<bool>
{
    public void Switch()
    {
        Set(!Value);
    }
}
