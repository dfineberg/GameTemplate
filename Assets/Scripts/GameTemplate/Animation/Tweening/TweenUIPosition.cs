using UnityEngine;

public class TweenUIPosition : AbstractTween<Vector2>
{
    private RectTransform _rt;

    protected override void Init()
    {
        _rt = transform as RectTransform;
    }

    public virtual Vector2 GetValue(float normalisedPoint)
    {
        return Vector2.Lerp(FromValue, ToValue, normalisedPoint);
    }

    protected override void SetValue(float normalisedPoint)
    {
        _rt.anchoredPosition = GetValue(normalisedPoint);
    }
}