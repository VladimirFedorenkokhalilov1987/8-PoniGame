using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class RunText : MonoBehaviour {
	[SerializeField, Range(0,1f),Tooltip("Adding symbol speed")]
	private float _speed = 1;

	#region Private Fields

	private Text _text;

	private Text Text {
		get { 
			if (!_text)
				_text =gameObject.GetComponent<Text> ();
			return _text;
			
		}
	}

	private string _originText;
	private float _timeToNext;

	#endregion
	void Start () {
		if (Text!=null) {
			_originText = Text.text;
			Text.text = string.Empty;
		}
		_timeToNext = Time.time + _speed;
	}
	
	void LateUpdate () {
		if (Time.time < _timeToNext)
			return;
		if (!Text) {
			throw new NullReferenceException ("Text is null");
		}

		int currentQuantity = Text.text.Length + 1;
		currentQuantity = currentQuantity>_originText.Length?0:currentQuantity;
		Text.text=_originText.Substring (0, currentQuantity);
		_timeToNext = Time.time + _speed;
	}
}
