using UnityEngine;
using UnityEngine.Events;

public class MonoEventReporter : MonoBehaviour {
	
	public enum MonoEvent { Awake, Start, OnEnable, OnDisable }

	public MonoEvent Event;
	public UnityEvent Callback;
	
	private void Awake()
	{
		if(Event == MonoEvent.Awake)
			Callback.Invoke();
	}

	private void Start()
	{
		if (Event == MonoEvent.Start)
			Callback.Invoke();
	}

	private void OnEnable()
	{
		if (Event == MonoEvent.OnEnable)
			Callback.Invoke();
	}

	private void OnDisable()
	{
		if(Event == MonoEvent.OnDisable)
			Callback.Invoke();
	}
}
