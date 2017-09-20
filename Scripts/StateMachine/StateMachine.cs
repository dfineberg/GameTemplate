using System.Collections;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class StateMachine : MonoBehaviour
{
    public string CurrentState;
    
    private AbstractState _currentState;

    private AbstractState _nextState;

    private bool _isRunning;


    public string CurrentStateName => _currentState?.GetType().ToString();

    public string NextStateName => _nextState?.GetType().ToString();

    public void Run(AbstractState firstState)
    {
        _currentState = firstState;
        _isRunning = true;
        StartCoroutine(StateMachineRoutine());
    }

    public void Stop()
    {
        _isRunning = false;
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

            if (!_isRunning)
                break;

            _currentState.ForceNextStateEvent -= HandleForceNextState;
            _currentState.OnExit();
            _currentState.Recycle();

            _currentState = _nextState;
            _nextState = null;
        }
    }

    private void HandleForceNextState(AbstractState forceState)
    {
        _currentState.ForceNextStateEvent -= HandleForceNextState;
        _currentState.OnExit();

        _currentState = forceState;

        _currentState.SetGameObject(gameObject);
        _currentState.ForceNextStateEvent += HandleForceNextState;
        _currentState.OnEnter();
    }

    private void FixedUpdate()
    {
        (_currentState as IFixedUpdate)?.FixedUpdate();
    }

    private void LateUpdate()
    {
        (_currentState as ILateUpdate)?.LateUpdate();
    }

    private void Update()
    {
        CurrentState = CurrentStateName;
        
        (_currentState as IUpdate)?.Update();
    }
}