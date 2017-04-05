using System.Collections.Generic;
using System.Linq;
using Promises;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public string GoogleMapsApiKey;
    public Vector2 DebugLocation;

    private StateMachine _stateMachine;

    [SerializeField] private AnimateOnOffGroup _loadingScreen;

    public static AnimateOnOffGroup LoadingScreen { get; private set; }

    public static SaveManager SaveManager { get; private set; }

    public static ProgressSaveManager ProgressSaveManager { get; private set; }

    public static Canvas Canvas { get; private set; }


    public static StringLibrary SceneDefsLibrary { get; private set; }

    public static StringLibrary BlockDefsLibrary { get; private set; }

    public static StringLibrary BuildingDefLibrary { get; private set; }

    public static EventSystem EventSystem { get; private set; }

    public static TouchInput TouchInput { get; private set; }

    public static Camera MainCamera { get; private set; }


    private void Awake()
    {
        LoadingScreen = _loadingScreen;
    }

    private void Start()
    {
        InitialLoadSequence()
            .ThenDo(RunStateMachine);
    }

    private void OnDisable()
    {
        ProgressSaveManager.SaveCurrentFile();
    }

    private IPromise InitialLoadSequence()
    {
        SaveManager = GetComponentInChildren<SaveManager>();

        if (!SaveManager)
            SaveManager = gameObject.AddComponent<SaveManager>();

        SaveManager.LoadSaveFile();

        CanvasExtensions.SetLoadingScreenTransform(LoadingScreen.transform);
        Canvas = GetComponentInChildren<Canvas>();

        var loadDefs = ResourceExtensions.LoadAllAsync(
                "Data/SceneLibrary",
                "Data/BlockLibrary",
                "Data/BuildingLibrary"
            )
            .ThenDo<Object[]>(o =>
            {
                SceneDefsLibrary = (StringLibrary)o[0];
                BlockDefsLibrary = (StringLibrary)o[1];
                BuildingDefLibrary = (StringLibrary)o[2];

                ProgressSaveManager = GetComponentInChildren<ProgressSaveManager>();

                if (!ProgressSaveManager)
                    ProgressSaveManager = gameObject.AddComponent<ProgressSaveManager>();

                ProgressSaveManager.LoadSaveFile();
            });

        EventSystem = FindObjectOfType<EventSystem>();

        TouchInput = gameObject.AddComponent<TouchInput>();

        MainCamera = Camera.main;

        _stateMachine = gameObject.AddComponent<StateMachine>();

        var artificialWait = Application.isEditor ? CoroutineExtensions.WaitForSeconds(1f) : Promise.Resolved();

        return Promise.All(loadDefs, artificialWait);
    }

    private void RunStateMachine()
    {
        _stateMachine.Run(new MainMenuState());
    }
}