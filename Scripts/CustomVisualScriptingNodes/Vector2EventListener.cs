using GameTemplate;
using Unity.VisualScripting;
using UnityEngine;

[UnitCategory("Events\\GameTemplate")]
public class Vector2EventListener : EventUnit<EmptyEventArgs>
{
    [DoNotSerialize, PortLabelHidden] public ValueInput Vector2Event;
    [DoNotSerialize] public ValueOutput Result;

    private bool _shouldTrigger;
    private Vector2 _result;
    
    protected override bool register => true;

    public override EventHook GetHook(GraphReference reference)
    {
        return new EventHook("FixedUpdate", reference.machine);
    }

    protected override void Definition()
    {
        base.Definition();
        Vector2Event = ValueInput<Vector2Event>("Vector2Event");
        Vector2Event.SetDefaultValue(null);

        Result = ValueOutput("Result", _ => _result);
    }

    public override void StartListening(GraphStack stack)
    {
        base.StartListening(stack);
        var graphReference = stack.ToReference();
        var vecEvent = Flow.FetchValue<Vector2Event>(Vector2Event, graphReference);
        if (vecEvent != null) vecEvent.Subscribe(OnBasicEvent);
    }

    public override void StopListening(GraphStack stack)
    {
        base.StopListening(stack);
        var graphReference = stack.ToReference();
        var vecEvent = Flow.FetchValue<Vector2Event>(Vector2Event, graphReference);
        if (vecEvent != null) vecEvent.Unsubscribe(OnBasicEvent);
    }

    private void OnBasicEvent(Vector2 value)
    {
        _shouldTrigger = true;
        _result = value;
    }

    protected override bool ShouldTrigger(Flow flow, EmptyEventArgs args)
    {
        if (_shouldTrigger)
        {
            _shouldTrigger = false;
            return true;
        }

        return false;
    }
}