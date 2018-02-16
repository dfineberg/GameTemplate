using GameTemplate;
using UnityEngine;

public class FloatValueBetweenListener : GenericValueListener<FloatValue, float>
{
    public float Min;
    public float Max;
    public GameObject SetActiveObject;
    
    protected override void Listener(float arg)
    {
        var active = arg >= Min && arg <= Max;
        
        if(SetActiveObject.activeSelf != active) SetActiveObject.SetActive(active);
    }
}
