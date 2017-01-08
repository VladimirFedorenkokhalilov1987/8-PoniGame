using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using PonyGame.Data;
using UnityEngine.SceneManagement;

public class UIController : StateListenerBase
{

	[SerializeField]
	private Timer _timer;
	[SerializeField]
	private Text _totalTime;
	[SerializeField]
	private Button _pausedButton;
	[SerializeField]
	private Button _resumeButton;
	[SerializeField]
	private Button _exitButton;


	protected override void OnStateChangedHandler(GameState arg1, GameState arg2) {
		if (arg1 == GameState.GameOver)
			ShowTotalTime ();
	}
	
	protected override void Awake () {
		if (_pausedButton)
			_pausedButton.onClick.AddListener (Pause);
		if (_resumeButton)
			_resumeButton.onClick.AddListener (Resume);
		if (_exitButton)
			_exitButton.onClick.AddListener (Exit);

		base.Awake();

	}
	protected override void OnDestroy () {
		if (_pausedButton)
			_pausedButton.onClick.RemoveListener (Pause);
		if (_resumeButton)
			_resumeButton.onClick.RemoveListener (Resume);
		if (_exitButton)
			_exitButton.onClick.RemoveListener (Exit);

		base.OnDestroy ();
	}

	private void Exit()
	{
		SceneManager.LoadScene (GameConfig.MenuSceneIndex);
	}

	private void Resume()
	{
		if (StateManager.Instanse)
			StateManager.Instanse.SetState (GameState.Resume);
	}

	private void Pause()
	{
		if (StateManager.Instanse)
			StateManager.Instanse.SetState (GameState.Pause);
	}

	private void ShowTotalTime()
	{
		if (!_timer||!_totalTime) throw new NullReferenceException ("Timer or time lbl not set");
		_totalTime.text = _timer.CurrentFormatedTime;
	}
}
