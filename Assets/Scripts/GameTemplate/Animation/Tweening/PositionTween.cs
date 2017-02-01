using System;
using System.Collections;
using System.Collections.Generic;
using Promises;
using UnityEngine;

[AddComponentMenu("Animation/Position Tween")]
public class PositionTween : AbstractTween<Vector3>
{
	public bool animateInWorldPosition;

    public override Vector3 GetValue(float normalisedPoint)
    {
        return Vector3.Lerp(fromValue, toValue, Easing.Interpolate(normalisedPoint, easeType));
    }

    protected override void SetValue(float normalisedPoint)
    {
        if(animateInWorldPosition)
			transform.position = GetValue(normalisedPoint);
		else
			transform.localPosition = GetValue(normalisedPoint);
    }
}
