namespace GameTemplate
{
    public abstract class AbstractPushState : AbstractStateBase
    {
        public bool Popped { get; private set; }

        protected void Pop()
        {
            Popped = true;
        }
    }
}