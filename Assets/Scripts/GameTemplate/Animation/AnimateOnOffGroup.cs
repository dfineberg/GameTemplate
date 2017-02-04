using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Promises;

public class AnimateOnOffGroup : MonoBehaviour {

	IAnimateOnOff[] _animations;

	float longestOn;
	float longestOff;

	void Awake()
	{
		_animations = GetComponentsInChildren<IAnimateOnOff>();

		foreach(var a in _animations)
		{
			longestOn = Mathf.Max(longestOn, a.onDuration);
			longestOff = Mathf.Max(longestOff, a.offDuration);
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
			CoroutineExtensions.WaitForSeconds(longestOff - a.offDuration)
			.Then(a.AnimateOff)
			)
		);
	}
}
