using System.Collections;
using System.Collections.Generic;
using Promises;
using UnityEngine;

public enum eAnimateOnOffState
{
    ON,
    OFF,
    ANIMATING_ON,
    ANIMATING_OFF
}

public interface IAnimateOnOff {

	IPromise AnimateOn();

	IPromise AnimateOff();

    void SetOn();

    void SetOff();

	float onDuration { get; }
	float offDuration { get; }

    eAnimateOnOffState currentState { get; }
}
