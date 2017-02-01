using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Promises;

public class AnimationGroup : MonoBehaviour {

	IAnimateOnOff[] _animations;

	void Awake()
	{
		_animations = GetComponentsInChildren<IAnimateOnOff>();
	}

	void OnEnable()
	{
		AnimateOn();
	}

	void OnDisable()
	{
		AnimateOff();
	}

	public IPromise AnimateOn()
	{
		return Promise.All(
			_animations.Select(a => a.AnimateOn())
			.ToArray()
		);
	}

	public IPromise AnimateOff()
	{
		return Promise.All(
			_animations.Select(a => a.AnimateOff())
			.ToArray()
		);
	}
}
