namespace GameTemplate
{
    public class BoolValueListener : GenericValueListener<BoolValue, bool>
    {
        public UnityEventBool Callback;

        protected override void Listener(bool b)
        {
            Callback.Invoke(b);
        }
    }
}
