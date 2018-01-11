using UnityEngine;

namespace GameTemplate
{
    public class BoolEventListener : MonoBehaviour
    {
        public BoolEvent BoolEvent;
        public UnityEventBool Callback;

        private void OnEnable()
        {
            BoolEvent?.Subscribe(Listener);
        }

        private void OnDisable()
        {
            BoolEvent?.Unsubscribe(Listener);
        }

        private void Listener(bool b)
        {
            Callback.Invoke(b);
        }
    }
}
