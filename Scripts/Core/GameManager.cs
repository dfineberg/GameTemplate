using System.Linq;
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

    public static TouchInput TouchInput { get; private set; }

    public static Camera MainCamera { get; private set; }
    
    public static PlanetDefinition[] PlanetDefinitions { get; private set; }
    
    public static ClothingDefinition ClothingLibrary { get; private set; }

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

        TouchInput = gameObject.AddComponent<TouchInput>();

        MainCamera = Camera.main;

        _stateMachine = gameObject.AddComponent<StateMachine>();
        
        return PlanetDefinition.LoadAllDefinitions()
            .ThenDo<object[]>(defs => PlanetDefinitions = defs.Cast<PlanetDefinition>().ToArray())
            .Then(() => ResourceExtensions.LoadAsync("Definitions/ClothingLibrary"))
            .ThenDo<ClothingDefinition>(lib => ClothingLibrary = lib);
    }

    private void RunStateMachine()
    {
        _stateMachine.Run(new CharacterSelectState());
    }
}