using UnityEngine;
using System.Collections;
using System.Linq;
using PonyGame.Data;
using System;

public class StateComponent : StateListenerBase {

	[SerializeField]
	private GameState[] _states;

	protected override void OnStateChangedHandler(GameState arg1, GameState arg2) {
		if(_states==null||_states.Length==0) throw new NullReferenceException ("StateManager.Instanse");

		var states = from i in _states
				where (i == arg1)
		             select i;

		bool condition = states == null || states.Count () == 0;

		gameObject.SetActive (!condition);
	}
}
