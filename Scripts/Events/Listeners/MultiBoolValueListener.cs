using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace GameTemplate
{
    public class MultiBoolValueListener : MonoBehaviour
    {
        public BoolValue[] Values;
        public bool FireOnEnable = true;
        public UnityEvent AllValuesTrue;
        public UnityEvent NotAllValuesTrue;

        private void OnEnable()
        {
            if (FireOnEnable) CheckCondition(false);
            foreach (var value in Values) value.Subscribe(CheckCondition);
        }

        private void OnDisable()
        {
            foreach (var value in Values) value.Unsubscribe(CheckCondition);
        }

        private void CheckCondition(bool b)
        {
            if (Values.All(v => v.Value)) AllValuesTrue.Invoke();
            else NotAllValuesTrue.Invoke();
        }
    }
}