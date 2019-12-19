using System;
using System.Collections;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace GameTemplate
{
    public class StateMachine : MonoBehaviour
    {    
        private AbstractState _currentState;
        private AbstractState _nextState;
        private IFixedUpdate _fixedUpdate;
        private ILateUpdate _lateUpdate;
        private IUpdate _update;

        private AbstractState _basePushState;

        public Action<Type> OnNewState;

        public bool IsRunning { get; private set; }
        public Type CurrentStateType => _currentState?.GetType();

        public void Run(AbstractState firstState)
        {
            SetCurrentState(firstState);
            IsRunning = true;
            StartCoroutine(StateMachineRoutine());
        }

        public void Stop()
        {
            IsRunning = false;

            _currentState.ForceNextStateEvent -= ForceNextState;
            _currentState.OnExit();
            SetCurrentState(null);
        }
        
        private void SetCurrentState(AbstractState newState)
        {
            _currentState = newState;
            SetUpdates(_currentState);
        }

        private void SetUpdates(AbstractStateBase abstractState)
        {
            _fixedUpdate = abstractState as IFixedUpdate;
            _lateUpdate = abstractState as ILateUpdate;
            _update = abstractState as IUpdate;
        }

        private IEnumerator StateMachineRoutine()
        {
            while (IsRunning)
            {
                Debug.Assert(_currentState != null, "Please provide a state");
                _currentState.ForceNextStateEvent += ForceNextState;
                _currentState.SetGameObject(gameObject);
                OnNewState?.Invoke(CurrentStateType);
                _currentState.OnEnter();

                while (_nextState == null && IsRunning)
                {
                    if (_currentState.PushState != null) yield return StartCoroutine(PushStateTransitionRoutine(_currentState));
                    _nextState = _currentState.NextState;
                    yield return null;
                }

                if (!IsRunning) yield break;

                _currentState.ForceNextStateEvent -= ForceNextState;
                _currentState.OnExit();

                SetCurrentState(_nextState);
                _nextState = null;
            }
        }

        public void ForceNextState(AbstractState forceState)
        {
            _currentState.ForceNextStateEvent -= ForceNextState;
            _currentState.OnExit();

            SetCurrentState(forceState);

            _currentState.SetGameObject(gameObject);
            _currentState.ForceNextStateEvent += ForceNextState;
            OnNewState?.Invoke(CurrentStateType);
            _currentState.OnEnter();
        }

        private IEnumerator PushStateRoutine(AbstractPushState pushState)
        {
            SetUpdates(pushState);
            pushState.SetGameObject(gameObject);
            pushState.OnEnter();
            
            while (!pushState.Popped)
            {
                if (pushState.PushState != null) yield return PushStateTransitionRoutine(pushState);
                yield return null;
            }
            
            pushState.OnExit();
        }

        private IEnumerator PushStateTransitionRoutine(AbstractStateBase currentState)
        {
            currentState.OnPushExit();
            yield return StartCoroutine(PushStateRoutine(currentState.PushState));
            currentState.ClearPushState();
            SetUpdates(currentState);
            currentState.OnPopEnter();
        }

        private void FixedUpdate()
        {
            _fixedUpdate?.FixedUpdate();
        }

        private void LateUpdate()
        {
            _lateUpdate?.LateUpdate();
        }

        private void Update()
        {
            _update?.Update();
        }
    }
}