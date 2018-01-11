using UnityEngine.Events;

namespace GameTemplate
{
    public class BasicEventListener : EventListener
    {
        public UnityEvent CallbackEvent;
    
        protected override void Callback()
        {
            CallbackEvent.Invoke();
        }
    }
}
