using GameTemplate.Promises;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameTemplate
{
    public abstract class GameManager<T, TU> : GameManagerCore where T : AbstractState, new() where TU : new()
    {
        private StateMachine _stateMachine;

        public TU SaveFile;
        
        protected abstract string SaveFileLocation { get; }

        private void Start()
        {
            Init();
            
            InitialLoadRoutine()
                .ThenDo(RunStateMachine);
        }

        protected virtual void OnDestroy()
        {
            ScreenshotUtility.Destroy();
        }

        protected virtual void Init()
        {
            LoadingScreen = GetComponentInChildren<AnimateOnOffGroup>();
            CanvasExtensions.SetLoadingScreenTransform(LoadingScreen.transform);
            
            Canvas = GetComponentInChildren<Canvas>();
            EventSystem = FindObjectOfType<EventSystem>();
            
            _stateMachine = gameObject.AddComponent<StateMachine>();
        }

        protected abstract IPromise InitialLoadRoutine();

        private void RunStateMachine()
        {
             _stateMachine.Run(new T());
        }

        public void LoadSaveFile()
        {
            SaveFile = ObjectSerialiser.LoadObjectAt<TU>(SaveFileLocation);

            if (SaveFile == null)
                SaveFile = new TU();
        }

        public void Save()
        {
            ObjectSerialiser.SaveObjectAt(SaveFile, SaveFileLocation);
        }
    }
}