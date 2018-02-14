namespace GameTemplate
{
    public class FloatValueListener : GenericValueListener<FloatValue, float>
    {
        public ValueListenerType Type = ValueListenerType.Any;
        public float ReferenceValue;
        public UnityEventFloat Callback;

        protected override void Listener(float arg)
        {
            switch (Type)
            {
                case ValueListenerType.LessThan:
                    if (arg < ReferenceValue) Callback.Invoke(arg);
                    break;
                case ValueListenerType.LessThanOrEquals:
                    if (arg <= ReferenceValue) Callback.Invoke(arg);
                    break;
                case ValueListenerType.GreaterThan:
                    if (arg > ReferenceValue) Callback.Invoke(arg);
                    break;
                case ValueListenerType.GreaterThanOrEquals:
                    if (arg >= ReferenceValue) Callback.Invoke(arg);
                    break;
                case ValueListenerType.Equals:
                    if (arg == ReferenceValue) Callback.Invoke(arg);
                    break;
                case ValueListenerType.Not:
                    if (arg != ReferenceValue) Callback.Invoke(arg);
                    break;
                case ValueListenerType.Any:
                    Callback.Invoke(arg);
                    break;
            }
        }
    }
}