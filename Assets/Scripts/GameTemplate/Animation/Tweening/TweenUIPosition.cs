using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenUIPosition : AbstractTween<Vector2>
{
    RectTransform _rt;

    protected override void Init()
    {
        _rt = transform as RectTransform;
    }

    public override Vector2 GetValue(float normalisedPoint)
    {
        return Vector2.Lerp(fromValue, toValue, normalisedPoint);
    }

    protected override void SetValue(float normalisedPoint)
    {
        _rt.anchoredPosition = GetValue(normalisedPoint);
    }
}
