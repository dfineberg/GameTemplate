namespace GameTemplate
{
    public abstract class AbstractState : AbstractStateBase
    {
        public event System.Action<AbstractState> ForceNextStateEvent;

        public AbstractState NextState { get; protected set; }

        protected void ForceNextState(AbstractState state)
        {
            NextState = state;

            ForceNextStateEvent?.Invoke(state);
        }
    }
}