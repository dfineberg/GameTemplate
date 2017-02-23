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
            yield return _currentState.OnEnter();

            while (_nextState == null && _isRunning)
            {
                _nextState = _currentState.NextState;
                yield return null;
            }

            if (!_isRunning)
                break;

            yield return _currentState.OnExit();

            _currentState = _nextState;
            _nextState = null;
        }
    }
}