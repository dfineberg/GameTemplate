using System;
using System.Collections;
using System.Collections.Generic;
using Promises;
using UnityEngine;

[AddComponentMenu("Animation/Position Tween")]
public class PositionTween : AbstractTween<Vector3>
{
	public bool animateInWorldPosition;

	void Start()
	{
		if(Application.isPlaying)
			Animate();
	}

    public override IPromise<AbstractTween<Vector3>> Animate()
    {
		var p = new Promise<AbstractTween<Vector3>>();

		CoroutineExtensions.WaitForSeconds(delay)
		.ThenTween(
			duration,
			f => SetValue(f)
		)
		.ThenDo(o => p.Resolve(this));

		return p;
    }

    public override Vector3 GetValueAtPoint(float normalisedPoint)
    {
        return Vector3.Lerp(fromValue, toValue, Easing.Interpolate(normalisedPoint, easeType));
    }

    protected override void SetValue(float normalisedPoint)
    {
        if(animateInWorldPosition)
			transform.position = GetValueAtPoint(normalisedPoint);
		else
			transform.localPosition = GetValueAtPoint(normalisedPoint);
    }
}
