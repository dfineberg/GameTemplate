using Promises;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	private StateMachine _stateMachine;

    public static AnimateOnOffGroup loadingScreen { get; private set; }

	void Awake()
	{
        loadingScreen = GetComponentInChildren<AnimateOnOffGroup>();
		_stateMachine = gameObject.AddComponent<StateMachine>();
	}

    void Start()
    {
        InitialLoadSequence()
            .ThenDo(RunStateMachine);
    }

    IPromise InitialLoadSequence()
    {
        return CoroutineExtensions.WaitForSeconds(1f);
    }

    void RunStateMachine()
    {
        _stateMachine.Run(new AnimateOnOffState());
    }
}
