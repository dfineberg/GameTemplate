using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Promises;

public class State {

	public State nextState { get; protected set; }

	public virtual IPromise OnEnter()
	{
		return Promise.Resolved();
	}

	public virtual IPromise OnExit()
	{
		return Promise.Resolved();
	}
}
