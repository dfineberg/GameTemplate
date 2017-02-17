using Promises;

public abstract class AbstractState
{
    public AbstractState NextState { get; protected set; }

    public abstract IPromise OnEnter();

    public abstract IPromise OnExit();
}