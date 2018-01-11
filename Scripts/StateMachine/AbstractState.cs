using UnityEngine;

// ReSharper disable InconsistentNaming

namespace GameTemplate
{
    public abstract class AbstractState
    {
        public event System.Action<AbstractState> ForceNextStateEvent;

        public AbstractState NextState { get; protected set; }

        protected GameObject gameObject { get; private set; }

        protected Transform transform => gameObject != null ? gameObject.transform : null;

        public abstract void OnEnter();

        public abstract void OnExit();

        public void SetGameObject(GameObject o)
        {
            gameObject = o;
        }

        public virtual void Recycle()
        {
            NextState = null;
        }

        protected void ForceNextState(AbstractState state)
        {
            NextState = state;

            ForceNextStateEvent?.Invoke(state);
        }
    }
}