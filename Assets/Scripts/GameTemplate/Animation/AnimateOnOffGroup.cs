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

            a.SetOff();
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
