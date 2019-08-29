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
        private bool _isRunning;

        private AbstractState _basePushState;

        public Action<string> OnNewState;

        public string CurrentStateName => _currentState?.GetType().ToString();

        public void Run(AbstractState firstState)
        {
            SetCurrentState(firstState);
            _isRunning = true;
            StartCoroutine(StateMachineRoutine());
        }

        public void Stop()
        {
            _isRunning = false;
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
            while (_isRunning)
            {
                Debug.Assert(_currentState != null, "Please provide a state");
                _currentState.ForceNextStateEvent += HandleForceNextState;
                _currentState.SetGameObject(gameObject);
                OnNewState?.Invoke(CurrentStateName);
                _currentState.OnEnter();

                while (_nextState == null && _isRunning)
                {
                    if (_currentState.PushState != null) yield return StartCoroutine(PushStateTransitionRoutine(_currentState));
                    _nextState = _currentState.NextState;
                    yield return null;
                }

                _currentState.ForceNextStateEvent -= HandleForceNextState;
                _currentState.OnExit();

                SetCurrentState(_nextState);
                _nextState = null;
            }
        }

        private void HandleForceNextState(AbstractState forceState)
        {
            _currentState.ForceNextStateEvent -= HandleForceNextState;
            _currentState.OnExit();

            SetCurrentState(forceState);

            _currentState.SetGameObject(gameObject);
            _currentState.ForceNextStateEvent += HandleForceNextState;
            OnNewState?.Invoke(CurrentStateName);
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