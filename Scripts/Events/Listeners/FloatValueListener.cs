namespace GameTemplate
{
	public class FloatValueListener : GenericValueListener<FloatValue, float>
	{
		public UnityEventFloat Callback;
		
		protected override void Listener(float arg)
		{
			Callback.Invoke(arg);
		}
	}
}