using GameTemplate.Promises;
using UnityEngine;

namespace GameTemplate
{
    [ExecuteInEditMode]
    public abstract class AbstractTween<T> : MonoBehaviour, IAnimateOnOff
    {
        [Range(0f, 1f)] public float TestPosition = 1f;

        public T FromValue;
        public T ToValue;

        public float Duration;
        public float Delay;

        public Easing.Functions EaseType;
    
        [SerializeField] private bool _ignoreAnimationGroup;

        private bool _initComplete;

        private EAnimateOnOffState _currentState = EAnimateOnOffState.Off;

        public float OnDuration
        {
            get { return Duration + Delay; }
        }

        public float OffDuration
        {
            get { return Duration + Delay; }
        }

        public AnimateOnOffGroup Group { get; set; }

        public bool IgnoreAnimationGroup
        {
            get { return _ignoreAnimationGroup; }
            set { _ignoreAnimationGroup = value; }
        }

        public EAnimateOnOffState CurrentState
        {
            get { return _currentState; }
        }

        protected abstract void SetValue(float normalisedPoint);

        protected virtual void Init()
        {
        }

        public IPromise AnimateOn(bool unscaled = false)
        {
            InitCheck();

            _currentState = EAnimateOnOffState.AnimatingOn;

            return CoroutineExtensions.WaitForSeconds(Delay, unscaled)
                .ThenTween(
                    Duration,
                    EaseType,
                    SetValue,
                    unscaled
                )
                .ThenDo(() => _currentState = EAnimateOnOffState.On);
        }

        public IPromise AnimateOff(bool unscaled = false)
        {
            InitCheck();

            _currentState = EAnimateOnOffState.AnimatingOff;

            return CoroutineExtensions.Tween(
                    Duration,
                    Easing.Reverse(EaseType),
                    f => SetValue(1f - f),
                    unscaled
                )
                .ThenWaitForSeconds(Delay, unscaled)
                .ThenDo(() => _currentState = EAnimateOnOffState.Off);
        }

        public void SetOn()
        {
            InitCheck();

            SetValue(1f);

            _currentState = EAnimateOnOffState.On;
        }

        public void SetOff()
        {
            InitCheck();

            SetValue(0f);

            _currentState = EAnimateOnOffState.Off;
        }

        private void Awake()
        {
            if(_ignoreAnimationGroup)
                SetOff();
        }

        protected void Update()
        {
            InitCheck();

            if (Application.isEditor && !Application.isPlaying)
                SetValue(Easing.Interpolate(TestPosition, EaseType));
        }

        private void InitCheck()
        {
            if (_initComplete) return;
            Init();
            _initComplete = true;
        }
    }
}