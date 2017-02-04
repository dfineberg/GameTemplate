using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour {

	private State _currentState;

	private State _nextState;

	private bool _isRunning = false;


	public string currentStateName
	{
		get
		{
			return _currentState == null ? string.Empty : _currentState.GetType().ToString();
		}
	}

	public string NextStateName
	{
		get
		{
			return _nextState == null ? string.Empty : _nextState.GetType().ToString();
		}
	}
	
	public void Run(State firstState)
	{
		_currentState = firstState;
		_isRunning = true;
		StartCoroutine(StateMachineRoutine());
	}

	public void Stop()
	{
		_isRunning = false;
	}

	IEnumerator StateMachineRoutine()
	{
		while(_isRunning)
		{
			yield return _currentState.OnEnter();

			while(_nextState == null && _isRunning)
			{
				_nextState = _currentState.nextState;
				yield return null;
			}

			if(!_isRunning)
				break;

			yield return _currentState.OnExit();

			_currentState = _nextState;
			_nextState = null;
		}
	}
}
