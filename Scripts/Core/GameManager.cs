using System;
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

    public static Camera MainCamera { get; private set; }
    
    public static PlanetDefinition[] PlanetDefinitions { get; private set; }
    
    public static ClothingDefinition ClothingLibrary { get; private set; }
    
    public static StringLibrary PuzzleLibrary { get; private set; }

    private void Awake()
    {
        LoadingScreen = _loadingScreen;
    }

    private void Start()
    {
        InitialLoadSequence()
            .ThenDo(() => Debug.Log(PlanetDefinitions == null ? "planet defs are null" : "found planet defs"))
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

        var loadPlanetDefs = PlanetDefinition.LoadAllDefinitions()
            .ThenDo<PlanetDefinition[]>(defs => PlanetDefinitions = defs);
        
        var loadClothingDefs = ResourceExtensions.LoadAsync("Definitions/ClothingLibrary")
            .ThenDo<ClothingDefinition>(lib => ClothingLibrary = lib);

        var loadPuzzleLib = ResourceExtensions.LoadAsync("Definitions/PuzzleLibrary")
            .ThenDo<StringLibrary>(lib => PuzzleLibrary = lib);
        
        return Promise.All(loadPlanetDefs, loadClothingDefs, loadPuzzleLib);
    }

    private void RunStateMachine()
    {
        _stateMachine.Run(new CharacterSelectState());
    }
}