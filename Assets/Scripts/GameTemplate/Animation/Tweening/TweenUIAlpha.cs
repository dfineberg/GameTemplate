using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TweenUIAlpha : AbstractTween<float>
{
    CanvasGroup _canvasGroup;
    Graphic _graphic;

    protected override void Init()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _graphic = GetComponent<Graphic>();
    }

    public override float GetValue(float normalisedPoint)
    {
        return Mathf.Lerp(fromValue, toValue, normalisedPoint);
    }

    protected override void SetValue(float normalisedPoint)
    {
        if(_canvasGroup)
        {
            _canvasGroup.alpha = GetValue(normalisedPoint);
            return;
        }

        Color c = _graphic.color;
        c.a = GetValue(normalisedPoint);
        _graphic.color = c;
    }
}
