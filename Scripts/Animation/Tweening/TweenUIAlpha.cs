using UnityEngine;
using UnityEngine.UI;

public class TweenUIAlpha : AbstractTween<float>
{
    private CanvasGroup _canvasGroup;
    private Graphic _graphic;

    protected override void Init()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _graphic = GetComponent<Graphic>();
    }

    public virtual float GetValue(float normalisedPoint)
    {
        return Mathf.Lerp(FromValue, ToValue, normalisedPoint);
    }

    protected override void SetValue(float normalisedPoint)
    {
        if (_canvasGroup)
        {
            _canvasGroup.alpha = GetValue(normalisedPoint);
            return;
        }

        var c = _graphic.color;
        c.a = GetValue(normalisedPoint);
        _graphic.color = c;
    }
}