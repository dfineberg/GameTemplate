using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Promises;
using System;

public class AnimateOnOffGroup : MonoBehaviour, IAnimateOnOff {

	IAnimateOnOff[] _animations;

	float _longestOn;
	float _longestOff;

    eAnimateOnOffState _currentState = eAnimateOnOffState.OFF;

    public float onDuration
    {
        get
        {
            return _longestOn;
        }
    }

    public float offDuration
    {
        get
        {
            return _longestOff;
        }
    }

    public eAnimateOnOffState currentState
    {
        get
        {
            return _currentState;
        }
    }

    void Awake()
	{
		_animations = GetComponentsInChildren<IAnimateOnOff>();

		foreach(var a in _animations)
		{
            if (a is AnimateOnOffGroup)
            {
                Debug.LogException(new Exception("Can't have an AnimateOnOffGroup in an AnimateOnOffGroup: " + gameObject.name));
                break;
            }

            _longestOn = Mathf.Max(_longestOn, a.onDuration);
			_longestOff = Mathf.Max(_longestOff, a.offDuration);
		}
	}

    public IPromise AnimateOn()
	{
		return Promise.All(_animations.SelectEach(a => a.AnimateOn()));
	}

	public IPromise AnimateOff()
	{
		return Promise.All(
			_animations.SelectEach(a => 
			CoroutineExtensions.WaitForSeconds(_longestOff - a.offDuration)
			.Then(a.AnimateOff)
			)
		);
	}

    public void SetOn()
    {
        foreach (var a in _animations)
            a.SetOn();
    }

    public void SetOff()
    {
        foreach (var a in _animations)
            a.SetOff();
    }
}
