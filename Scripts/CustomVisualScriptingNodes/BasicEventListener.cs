using GameTemplate;
using Unity.VisualScripting;

[UnitCategory("Events\\GameTemplate")]
public class BasicEventListener : EventUnit<EmptyEventArgs>
{
    [DoNotSerialize, PortLabelHidden] public ValueInput BasicEvent;

    private bool _shouldTrigger;
    
    protected override bool register => true;

    public override EventHook GetHook(GraphReference reference)
    {
        return new EventHook("FixedUpdate", reference.machine);
    }

    protected override void Definition()
    {
        base.Definition();
        BasicEvent = ValueInput<BasicEvent>("BasicEvent");
        BasicEvent.SetDefaultValue(null);
    }

    public override void StartListening(GraphStack stack)
    {
        base.StartListening(stack);
        var graphReference = stack.ToReference();
        var bEvent = Flow.FetchValue<BasicEvent>(BasicEvent, graphReference);
        if (bEvent != null) bEvent.Subscribe(OnBasicEvent);
    }

    public override void StopListening(GraphStack stack)
    {
        base.StopListening(stack);
        var graphReference = stack.ToReference();
        var bEvent = Flow.FetchValue<BasicEvent>(BasicEvent, graphReference);
        if (bEvent != null) bEvent.Unsubscribe(OnBasicEvent);
    }

    private void OnBasicEvent()
    {
        _shouldTrigger = true;
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
