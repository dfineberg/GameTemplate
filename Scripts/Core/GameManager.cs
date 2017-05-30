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

    public static EventSystem EventSystem { get; private set; }

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

        EventSystem = FindObjectOfType<EventSystem>();

        MainCamera = Camera.main;

        _stateMachine = gameObject.AddComponent<StateMachine>();
        return CoroutineExtensions.WaitForSeconds(1f);
    }

    private void RunStateMachine()
    {
        // _stateMachine.Run( FIRST STATE GOES HERE );
    }
}