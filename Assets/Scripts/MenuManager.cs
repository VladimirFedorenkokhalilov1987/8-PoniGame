using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using PonyGame.Data;



public class MenuManager : MonoBehaviour {
	[SerializeField]
	private Text _score;
	void Start () {
		SetScoreValue ();
	}

	public void LoadGameScene()
	{
		SceneManager.LoadScene (GameConfig.GameSceneIndex);
	}

	void SetScoreValue () {
		if (!_score) {
			throw new NullReferenceException ("Score lbl is null.");

			string score = PlayerPrefs.GetString (GameConfig.ScoreKeyFormatted);
			if (string.IsNullOrEmpty(score)) {
				score = "99.99";
			}
			_score.text = score;
		}
	}
}
