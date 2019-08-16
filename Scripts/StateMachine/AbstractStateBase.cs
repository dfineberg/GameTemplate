using UnityEngine;
// ReSharper disable InconsistentNaming

namespace GameTemplate
{
    public abstract class AbstractStateBase
    {
        public AbstractPushState PushState { get; protected set; }
        protected GameObject gameObject { get; private set; }
        protected Transform transform => gameObject != null ? gameObject.transform : null;
        public abstract void OnEnter();
        public abstract void OnExit();
        public virtual void OnPushExit(){}
        public virtual void OnPopEnter(){}

        public void SetGameObject(GameObject o)
        {
            gameObject = o;
        }

        public void ClearPushState()
        {
            PushState = null;
        }
    }
}