using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Promises;
using System;

public class AnimateOnOffGroup : MonoBehaviour, IAnimateOnOff {

    public float onDelay;
    public float offDelay;

    [SerializeField]
    eAnimateOnOffState _awakeState = eAnimateOnOffState.OFF;

	List<IAnimateOnOff> _animations;

	float _longestOn;
	float _longestOff;

    eAnimateOnOffState _currentState = eAnimateOnOffState.OFF;

    public float onDuration
    {
        get
        {
            return onDelay + _longestOn;
        }
    }

    public float offDuration
    {
        get
        {
            return offDelay + _longestOff;
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
        _animations = new List<IAnimateOnOff>();

		foreach(var a in GetComponentsInChildren<IAnimateOnOff>())
		{
            if (a == this)
                continue;

            if (a is AnimateOnOffGroup)
            {
                Debug.LogException(new Exception("Can't have an AnimateOnOffGroup in an AnimateOnOffGroup: " + gameObject.name));
                break;
            }

            _longestOn = Mathf.Max(_longestOn, a.onDuration);
			_longestOff = Mathf.Max(_longestOff, a.offDuration);

            _animations.Add(a);
		}

        switch(_awakeState)
        {
            case eAnimateOnOffState.ANIMATING_OFF:
                AnimateOff();
                break;
            case eAnimateOnOffState.ANIMATING_ON:
                AnimateOn();
                break;
            case eAnimateOnOffState.OFF:
                SetOff();
                break;
            case eAnimateOnOffState.ON:
                SetOn();
                break;
        }
	}

    public IPromise AnimateOn()
	{
        return CoroutineExtensions.WaitForSeconds(onDelay)
            .ThenAll(() => GetAnimateOnPromises());
	}

	public IPromise AnimateOff()
	{
        return CoroutineExtensions.WaitForSeconds(offDelay)
            .ThenLog("off delay complete")
            .ThenAll(() => GetAnimateOffPromises());
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

    private IEnumerable<IPromise> GetAnimateOnPromises()
    {
        return _animations.SelectEach(
                a => a.AnimateOn()
            );
    }

    private IEnumerable<IPromise> GetAnimateOffPromises()
    {
        return _animations.SelectEach(
                a => CoroutineExtensions.WaitForSeconds(_longestOff - a.offDuration)
                .Then(a.AnimateOff)
            );
    }
}
