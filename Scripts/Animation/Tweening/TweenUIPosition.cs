using UnityEngine;

public class TweenUIPosition : AbstractTween<Vector2>
{
    private RectTransform _rt;

    protected override void Init()
    {
        _rt = transform as RectTransform;
    }

    protected override void SetValue(float normalisedPoint)
    {
        _rt.anchoredPosition = Vector2.LerpUnclamped(FromValue, ToValue, Easing.Interpolate(normalisedPoint, EaseType));
    }
}