using UnityEngine;

namespace GameTemplate
{
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

        protected override bool Equals(float other)
        {
            return Value == other;
        }
    }
}
