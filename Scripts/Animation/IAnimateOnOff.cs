﻿using Promises;

public enum EAnimateOnOffState
{
    On,
    Off,
    AnimatingOn,
    AnimatingOff
}

public interface IAnimateOnOff
{
    IPromise AnimateOn();

    IPromise AnimateOff();

    void SetOn();

    void SetOff();

    float OnDuration { get; }
    float OffDuration { get; }

    EAnimateOnOffState CurrentState { get; }
}