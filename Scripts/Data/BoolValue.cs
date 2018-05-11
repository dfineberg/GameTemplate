using UnityEngine;

namespace GameTemplate
{
    [CreateAssetMenu(menuName = "Data/BoolValue")]
    public class BoolValue : GenericValue<bool>
    {
        public void Switch()
        {
            Value = !Value;
        }

        protected override bool Equals(bool other)
        {
            return Value == other;
        }
    }
}
