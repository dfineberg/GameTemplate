using System;
using System.Collections.Generic;
using System.Linq;
using Promises;
using UnityEngine;

public class AnimateOnOffGroup : MonoBehaviour, IAnimateOnOff
{
    public float OnDelay;
    public float OffDelay;

    [SerializeField] private EAnimateOnOffState _awakeState = EAnimateOnOffState.Off;

    private List<IAnimateOnOff> _animations;

    private float _longestOn;
    private float _longestOff;

    public float OnDuration
    {
        get { return OnDelay + _longestOn; }
    }

    public float OffDuration
    {
        get { return OffDelay + _longestOff; }
    }

    public EAnimateOnOffState CurrentState { get; private set; }

    private void Awake()
    {
        CurrentState = EAnimateOnOffState.Off;

        _animations = new List<IAnimateOnOff>();

        foreach (var a in GetComponentsInChildren<IAnimateOnOff>())
        {
            if (ReferenceEquals(a, this))
                continue;

            if (a is AnimateOnOffGroup)
            {
                Debug.LogException(new Exception("Can't have an AnimateOnOffGroup in an AnimateOnOffGroup: " +
                                                 gameObject.name));
                break;
            }

            _longestOn = Mathf.Max(_longestOn, a.OnDuration);
            _longestOff = Mathf.Max(_longestOff, a.OffDuration);

            _animations.Add(a);
        }

        switch (_awakeState)
        {
            default:
                SetOn();
                break;
            case EAnimateOnOffState.Off:
                SetOff();
                break;
            case EAnimateOnOffState.AnimatingOn:
                AnimateOn();
                break;
            case EAnimateOnOffState.AnimatingOff:
                AnimateOff();
                break;
        }
    }

    public IPromise AnimateOn()
    {
        if (CheckIfAnimating())
            return Promise.Resolved();

        if (CurrentState == EAnimateOnOffState.On)
            return Promise.Resolved();

        CurrentState = EAnimateOnOffState.AnimatingOn;

        gameObject.SetActive(true);

        return CoroutineExtensions.WaitForSeconds(OnDelay)
            .ThenAll(() => GetAnimateOnPromises())
            .ThenDo(() => CurrentState = EAnimateOnOffState.On);
    }

    public IPromise AnimateOff()
    {
        if (CheckIfAnimating())
            return Promise.Resolved();

        if (CurrentState == EAnimateOnOffState.Off)
            return Promise.Resolved();

        CurrentState = EAnimateOnOffState.AnimatingOff;

        return CoroutineExtensions.WaitForSeconds(OffDelay)
            .ThenAll(() => GetAnimateOffPromises())
            .ThenDo(() => CurrentState = EAnimateOnOffState.Off);
    }

    public void SetOn()
    {
        CurrentState = EAnimateOnOffState.On;
        gameObject.SetActive(true);

        foreach (var a in _animations)
            a.SetOn();
    }

    public void SetOff()
    {
        CurrentState = EAnimateOnOffState.Off;

        foreach (var a in _animations)
            a.SetOff();
    }

    private IEnumerable<IPromise> GetAnimateOnPromises()
    {
        return _animations.SelectEach(
            a => a.AnimateOn()
        );
    }

    private IEnumerable<IPromise> GetAnimateOffPromises()
    {
        return _animations.SelectEach(
            a => CoroutineExtensions.WaitForSeconds(_longestOff - a.OffDuration)
                .Then(a.AnimateOff)
        );
    }

    private bool CheckIfAnimating()
    {
        if (CurrentState == EAnimateOnOffState.AnimatingOn || CurrentState == EAnimateOnOffState.AnimatingOff)
        {
            Debug.LogError("Animating while we're animating: " + gameObject.name);
            return true;
        }

        return false;
    }
}