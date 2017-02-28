using Promises;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    private StateMachine _stateMachine;

    [SerializeField] private AnimateOnOffGroup _loadingScreen;

    public static AnimateOnOffGroup LoadingScreen { get; private set; }

    public static SaveManager SaveManager { get; private set; }

    public static Canvas Canvas { get; private set; }


    public static StringLibrary SceneDefsLibrary { get; private set; }

    public static StringLibrary BlockDefsLibrary { get; private set; }

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

    private IPromise InitialLoadSequence()
    {
        SaveManager = GetComponentInChildren<SaveManager>();

        if (!SaveManager)
            SaveManager = gameObject.AddComponent<SaveManager>();

        SaveManager.LoadSaveFile();

        CanvasExtensions.SetLoadingScreenTransform(LoadingScreen.transform);
        Canvas = GetComponentInChildren<Canvas>();

        var loadDefs = ResourceExtensions.LoadAllAsync<StringLibrary>(
            new[] {"Data/SceneLibrary", "Data/BlockLibrary"},
            o =>
            {
                SceneDefsLibrary = o[0];
                BlockDefsLibrary = o[1];
            }
        );

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