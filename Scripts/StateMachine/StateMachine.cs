using System;
using System.Collections;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private AbstractState _currentState;

    private AbstractState _nextState;

    private bool _isRunning;


    public string CurrentStateName
    {
        get { return _currentState == null ? string.Empty : _currentState.GetType().ToString(); }
    }

    public string NextStateName
    {
        get { return _nextState == null ? string.Empty : _nextState.GetType().ToString(); }
    }

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
        if(_currentState is IFixedUpdate)
            (_currentState as IFixedUpdate).FixedUpdate();
    }

    private void LateUpdate()
    {
        if (_currentState is ILateUpdate)
            (_currentState as ILateUpdate).LateUpdate();
    }

    private void Update()
    {
        if (_currentState is IUpdate)
            (_currentState as IUpdate).Update();
    }
}