using System.Collections;
using System.Collections.Generic;
using Promises;
using UnityEngine;

[ExecuteInEditMode]
public abstract class AbstractTween<T> : MonoBehaviour {

	[RangeAttribute(0f, 1f)]
	public float testPosition;

	public T fromValue;
	public T toValue;

	public float duration;
	public float delay;

	public Easing.Functions easeType;


	public abstract IPromise<AbstractTween<T>> Animate();

	public abstract T GetValueAtPoint(float normalisedPoint);

	protected abstract void SetValue(float normalisedPoint);

	protected virtual void Update()
	{
		if(Application.isEditor && !Application.isPlaying)
			SetValue(testPosition);
	}
}
