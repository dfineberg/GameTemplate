using Promises;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private StateMachine _stateMachine;

    [SerializeField]
    private AnimateOnOffGroup _loadingScreen;

    public static AnimateOnOffGroup loadingScreen { get; private set; }

    public static SaveManager saveManager { get; private set; }

    public static Canvas canvas { get; private set; }

	void Awake()
	{
        loadingScreen = _loadingScreen;
	}

    void Start()
    {
        InitialLoadSequence()
            .ThenDo(RunStateMachine);
    }

    IPromise InitialLoadSequence()
    {
        saveManager = GetComponentInChildren<SaveManager>();

        if (!saveManager)
            saveManager = gameObject.AddComponent<SaveManager>();

        saveManager.LoadSaveFile();

        CanvasExtensions.SetLoadingScreenTransform(loadingScreen.transform);
        canvas = GetComponentInChildren<Canvas>();

        _stateMachine = gameObject.AddComponent<StateMachine>();
        return CoroutineExtensions.WaitForSeconds(1f);
    }

    void RunStateMachine()
    {
        _stateMachine.Run(new TitleScreenState());
    }
}
