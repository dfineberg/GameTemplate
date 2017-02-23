using Promises;
using UnityEngine;

[ExecuteInEditMode]
public abstract class AbstractTween<T> : MonoBehaviour, IAnimateOnOff
{
    [Range(0f, 1f)] public float TestPosition;

    public T FromValue;
    public T ToValue;

    public float Duration;
    public float Delay;

    public Easing.Functions EaseType;

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

    public EAnimateOnOffState CurrentState
    {
        get { return _currentState; }
    }

    protected abstract void SetValue(float normalisedPoint);

    protected virtual void Init()
    {
    }

    public IPromise AnimateOn()
    {
        InitCheck();

        _currentState = EAnimateOnOffState.AnimatingOn;

        return CoroutineExtensions.WaitForSeconds(Delay)
            .ThenTween(
                Duration,
                EaseType,
                SetValue
            )
            .ThenDo(() => _currentState = EAnimateOnOffState.On);
    }

    public IPromise AnimateOff()
    {
        InitCheck();

        _currentState = EAnimateOnOffState.AnimatingOff;

        return CoroutineExtensions.Tween(
                Duration,
                Easing.Reverse(EaseType),
                f => SetValue(1f - f)
            )
            .ThenWaitForSeconds(Delay)
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