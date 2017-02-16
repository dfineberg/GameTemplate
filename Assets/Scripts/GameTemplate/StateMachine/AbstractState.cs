using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Promises;

public abstract class AbstractState {

	public AbstractState nextState { get; protected set; }

    public abstract IPromise OnEnter();

    public abstract IPromise OnExit();
}
