using UnityEngine;
using System.Collections;
using System;
using PonyGame.Data;

public abstract class StateListenerBase : MonoBehaviour {

	protected virtual void Awake () {
		if (StateManager.Instanse)
			StateManager.Instanse.OnStateChanged += OnStateChangedHandler;
		else
			throw new NullReferenceException ("StateManager.Instanse");
	}

	protected virtual void OnDestroy()
	{
		if (StateManager.Instanse)
			StateManager.Instanse.OnStateChanged -= OnStateChangedHandler;
	}

	protected abstract void OnStateChangedHandler (GameState arg1, GameState arg2);
}
