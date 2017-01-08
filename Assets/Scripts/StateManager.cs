using UnityEngine;
using System.Collections;
using System;
using PonyGame.Data;
namespace PonyGame.Data
{
	public enum GameState
	{
		None,
		Play,
		Pause,
		Resume,
		GameOver
	}
}


public class StateManager : MonoBehaviour {
	private static StateManager _instance;

	public static StateManager Instanse
	{
		get
		{ 
			return _instance;
		}
	}

	private void Awake()
	{
		if (_instance == null)
			_instance = this;
		else
			DestroyImmediate (gameObject);

	}
		
	[SerializeField]
	private GameState _currentState;
	[SerializeField]
	private GameState _previousState;

	public event Action <GameState,GameState> OnStateChanged;

	private void OnStateChangedHandler(GameState current,GameState previus)
	{
		if (OnStateChanged != null)
			OnStateChanged (current, previus);
	}

	public void SetState(GameState newState)
	{
		_previousState = _currentState;
		_currentState = newState;
		OnStateChangedHandler (_currentState, _previousState);
	}

//	private void OnGUI()
//	{
//		if(GUI.Button(new Rect (10,10,100,50), "Play")) SetState(GameState.Play);
//		if(GUI.Button(new Rect (10,60,150,50), "Pause")) SetState(GameState.Pause);
//		if(GUI.Button(new Rect (10,110,200,50), "Resume")) SetState(GameState.Resume);
//		if(GUI.Button(new Rect (10,160,250,50), "GameOver")) SetState(GameState.GameOver);
//	}
}
