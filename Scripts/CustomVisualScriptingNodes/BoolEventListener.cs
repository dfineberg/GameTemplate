using GameTemplate;
using Unity.VisualScripting;

[UnitCategory("Events\\GameTemplate")]
public class BoolEventListener : EventUnit<EmptyEventArgs>
{
    [DoNotSerialize, PortLabelHidden] public ValueInput BoolEvent;
    [DoNotSerialize] public ValueOutput Result;

    private bool _shouldTrigger;
    private bool _result;
    
    protected override bool register => true;

    public override EventHook GetHook(GraphReference reference)
    {
        return new EventHook("FixedUpdate", reference.machine);
    }

    protected override void Definition()
    {
        base.Definition();
        BoolEvent = ValueInput<BoolEvent>("BoolEvent");
        BoolEvent.SetDefaultValue(null);

        Result = ValueOutput("Result", _ => _result);
    }

    public override void StartListening(GraphStack stack)
    {
        base.StartListening(stack);
        var graphReference = stack.ToReference();
        var bEvent = Flow.FetchValue<BoolEvent>(BoolEvent, graphReference);
        if (bEvent != null) bEvent.Subscribe(OnBasicEvent);
    }

    public override void StopListening(GraphStack stack)
    {
        base.StopListening(stack);
        var graphReference = stack.ToReference();
        var bEvent = Flow.FetchValue<BoolEvent>(BoolEvent, graphReference);
        if (bEvent != null) bEvent.Unsubscribe(OnBasicEvent);
    }

    private void OnBasicEvent(bool b)
    {
        _shouldTrigger = true;
        _result = b;
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