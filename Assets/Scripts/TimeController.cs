using UnityEngine;
using PonyGame.Data;
using System.Collections;
using System;
public class TimeController : StateListenerBase {

	private Timer _timer;
	private Timer Timer
	{
		get
		{ 
			if (_timer == null)
				_timer = GetComponent<Timer> ();
			return _timer;
		}
	}

	protected override void OnStateChangedHandler(GameState arg1, GameState arg2)
	{
		if(!Timer) throw new NullReferenceException ("Timer is null");

		switch (arg1) {
		case GameState.Play:
			Timer.ON ();
			break;

		case GameState.Pause:
			Timer.Pause ();
			break;

		case GameState.Resume:
			Timer.Resume ();
			break;

		case GameState.GameOver:
			Timer.OFF ();
			SaveScore ();
			break;

		default:
			throw new ArgumentOutOfRangeException(string.Format ("{0} not found",arg1));
			break;
		}

	}

	// Use this for initialization
	void SaveScore () {
		if(!Timer) throw new NullReferenceException ("Timer is null");

		float prevScore = PlayerPrefs.GetFloat (GameConfig.ScoreKey);
		if (Timer.CurrentTime >= prevScore && prevScore > 0)
			return;

		PlayerPrefs.SetFloat (GameConfig.ScoreKey, Timer.CurrentTime);
		PlayerPrefs.SetString (GameConfig.ScoreKeyFormatted, Timer.CurrentFormatedTime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
