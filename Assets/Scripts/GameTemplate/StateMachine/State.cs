using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Promises;

public abstract class State {

	public State nextState { get; protected set; }

    public abstract IPromise OnEnter();

    public abstract IPromise OnExit();
}
