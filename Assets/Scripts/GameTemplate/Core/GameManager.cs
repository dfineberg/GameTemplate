using Promises;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private StateMachine _stateMachine;

    [SerializeField]
    private AnimateOnOffGroup _loadingScreen;

    public static AnimateOnOffGroup loadingScreen { get; private set; }

    public static Canvas canvas { get; private set; }

	void Awake()
	{
        loadingScreen = _loadingScreen;
        CanvasExtensions.SetLoadingScreenTransform(loadingScreen.transform);
        canvas = GetComponentInChildren<Canvas>();
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
        _stateMachine.Run(new TitleScreenState());
    }
}
