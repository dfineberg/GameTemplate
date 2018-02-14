namespace GameTemplate
{
    public class BoolValueListener : GenericValueListener<BoolValue, bool>
    {
        public enum ListenerType { Equals, NotEquals, Any, AnyInverse }

        public ListenerType Type = ListenerType.Any;
        public bool ReferenceValue;
        public UnityEventBool Callback;

        protected override void Listener(bool b)
        {
            switch (Type)
            {
                case ListenerType.Equals:
                    if(b == ReferenceValue) Callback.Invoke(b);
                    break;
                case ListenerType.NotEquals:
                    if(b != ReferenceValue) Callback.Invoke(b);
                    break;
                case ListenerType.Any:
                    Callback.Invoke(b);
                    break;
                case ListenerType.AnyInverse:
                    Callback.Invoke(!b);
                    break;
            }
        }
    }
}
