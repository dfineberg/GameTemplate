using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	private StateMachine _stateMachine;

	void Awake()
	{
		_stateMachine = gameObject.AddComponent<StateMachine>();

		_stateMachine.Run(new AnimateOnOffState());
	}
}
