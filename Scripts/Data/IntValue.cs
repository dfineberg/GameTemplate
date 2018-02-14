using UnityEngine;

namespace GameTemplate
{
    [CreateAssetMenu(menuName = "Data/IntValue")]
    public class IntValue : GenericValue<int>
    {
        public void Add(int i)
        {
            Value += i;
        }

        public void Increment()
        {
            Value++;
        }

        public void Decrement()
        {
            Value--;
        }
    }
}