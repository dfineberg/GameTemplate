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

    private bool _initComplete = false;

    private eAnimateOnOffState _currentState = eAnimateOnOffState.OFF;

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

    public eAnimateOnOffState currentState
    {
        get
        {
            return _currentState;
        }
    }

    public abstract T GetValue(float normalisedPoint);

	protected abstract void SetValue(float normalisedPoint);

    protected virtual void Init() { }

	public IPromise AnimateOn()
	{
        InitCheck();

        _currentState = eAnimateOnOffState.ANIMATING_ON;

        return CoroutineExtensions.WaitForSeconds(delay)
        .ThenTween(
            duration,
            easeType,
            f => SetValue(f)
        )
        .ThenDo(() => _currentState = eAnimateOnOffState.ON);
	}

	public IPromise AnimateOff()
	{
        InitCheck();

        _currentState = eAnimateOnOffState.ANIMATING_OFF;

        return CoroutineExtensions.Tween(
            duration,
            Easing.Reverse(easeType),
            f => SetValue(1f - f)
        )
        .ThenWaitForSeconds(delay)
        .ThenDo(() => _currentState = eAnimateOnOffState.OFF);
    }

    public void SetOn()
    {
        InitCheck();

        SetValue(1f);

        _currentState = eAnimateOnOffState.ON;
    }

    public void SetOff()
    {
        InitCheck();

        SetValue(0f);

        _currentState = eAnimateOnOffState.OFF;
    }

    protected virtual void Update()
	{
        InitCheck();

		if(Application.isEditor && !Application.isPlaying)
			SetValue(Easing.Interpolate(testPosition, easeType));
	}

    private void InitCheck()
    {
        if (!_initComplete)
        {
            Init();
            _initComplete = true;
        }
    }
}
