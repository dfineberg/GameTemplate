using System.Collections;
using System.Collections.Generic;
using GameTemplate;
using UnityEngine;

public class FloatTween : AbstractTween<float>
{
    public UnityEventFloat Callback;
    
    protected override void SetValue(float normalisedPoint)
    {
        Callback.Invoke(Mathf.Lerp(FromValue, ToValue, normalisedPoint));
    }
}
