using System.Collections;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace GameTemplate
{
    public class StateMachine : MonoBehaviour
    {
        public string CurrentState;
    
        private AbstractState _currentState;
        private AbstractState _nextState;
        private IFixedUpdate _fixedUpdate;
        private ILateUpdate _lateUpdate;
        private IUpdate _update;
        private bool _isRunning;


        public string CurrentStateName => _currentState?.GetType().ToString();

        public string NextStateName => _nextState?.GetType().ToString();

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
            _fixedUpdate = _currentState as IFixedUpdate;
            _lateUpdate = _currentState as ILateUpdate;
            _update = _currentState as IUpdate;
        }

        private IEnumerator StateMachineRoutine()
        {
            while (_isRunning)
            {
                Debug.Assert(_currentState != null, "Please provide a state");
                _currentState.ForceNextStateEvent += HandleForceNextState;
                _currentState.SetGameObject(gameObject);
                _currentState.OnEnter();

                while (_nextState == null && _isRunning)
                {
                    _nextState = _currentState.NextState;
                    yield return null;
                }

                _currentState.ForceNextStateEvent -= HandleForceNextState;
                _currentState.OnExit();
                _currentState.Recycle();

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
            _currentState.OnEnter();
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
            CurrentState = CurrentStateName;
            _update?.Update();
        }
    }
}