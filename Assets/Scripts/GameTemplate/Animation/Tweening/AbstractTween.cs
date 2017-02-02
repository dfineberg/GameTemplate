using System;
using System.Collections;
using System.Collections.Generic;
using Promises;
using UnityEngine;

[ExecuteInEditMode]
public abstract class AbstractTween<T> : MonoBehaviour, IAnimateOnOff {

	[RangeAttribute(0f, 1f)]
	public float testPosition;

	public T fromValue;
	public T toValue;

	public float duration;
	public float delay;

	public Easing.Functions easeType;

    public float onDuration
    {
        get
        {
            return duration + delay;
        }
    }

    public float offDuration
    {
        get
        {
            return duration + delay;
        }
    }

    public abstract T GetValue(float normalisedPoint);

	protected abstract void SetValue(float normalisedPoint);

	public IPromise AnimateOn()
	{
		return CoroutineExtensions.WaitForSeconds(delay)
		.ThenTween(
			duration,
			f => SetValue(f)
		);
	}

	public IPromise AnimateOff()
	{
		return CoroutineExtensions.Tween(
			duration,
			f => SetValue(1f - f)
		)
		.ThenWaitForSeconds(delay);
	}

	protected virtual void Update()
	{
		if(Application.isEditor && !Application.isPlaying)
			SetValue(testPosition);
	}
}
