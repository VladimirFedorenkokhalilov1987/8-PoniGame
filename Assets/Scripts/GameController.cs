using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using UnityEngine.UI;
using PonyGame.Data;
using UnityEngine.SceneManagement;
using UnityRandom = UnityEngine.Random;

public class GameController : MonoBehaviour {

	[SerializeField]
	private InputManager _inputManager;

	[SerializeField]
	private FarmManager _farmManager;

	[SerializeField]
	private int _dogsQuantity=4;
	[SerializeField]
	private int _ponyQuantity=4;

	[SerializeField]
	private int _ponyBonusQuantity=2;
	[SerializeField]
	private SheepDog _sheepDogPrefab;

	[SerializeField]
	private Pony _ponyPrefab;



	[SerializeField]
	private float _filedOffset =1;
	[SerializeField]
	private float _gameOverDelay =2f;

	private SheepDog[] _dogs;
	private Pony[] _ponys;
	private SheepDog _selectedDog;


	// Use this for initialization
	void Start () {
		if(_inputManager) _inputManager.OnMouseDown+= _inputManager_OnMouseDown;
		if(_farmManager)_farmManager.RecivedPonys+= _farmManager_RecivedPonys;
		SetState (GameState.Play);
		GenerateHerd ();
	}

	void _farmManager_RecivedPonys (int obj)
	{
		if (_ponys.Length == 0)
			return;
		var inFarmPony = from i in _ponys
		                where i.IsInFarm
		                select i;
		
		if (inFarmPony.Count () == _ponys.Length) {
			if (IsInvoking ("GameOver"))
				return;
			Invoke ("GameOver", _gameOverDelay);
			return;
		}
	}

	private void GameOver()
	{
		SetState (GameState.GameOver);
	}

	void _inputManager_OnMouseDown (RaycastHit2D obj)
	{
		if(_dogs==null||_dogs.Length==0)throw new NullReferenceException ("_dogs is null");

		if (!obj.collider) {
			if (_selectedDog)
				_selectedDog.MoveToPoint (obj.point);
			return;
		}

		var selected = from i in _dogs
		               where i.gameObject == obj.collider.gameObject
		               select i;
		
		if (selected == null || selected.Count () == 0) {
			if(_selectedDog)_selectedDog.MoveToPoint(obj.point);
			return;
		}

		var _newSelectedDog = selected.FirstOrDefault ();
		if (_newSelectedDog == _selectedDog && _selectedDog) {
			_selectedDog.UnSelect ();
			_selectedDog.StopMoving ();
			_selectedDog = null;
			return;
		}
		if (_newSelectedDog)
			_newSelectedDog.Select ();
		if (_selectedDog)
			_selectedDog.UnSelect ();
		_selectedDog = _newSelectedDog;

	}
	
	// Update is called once per frame
	void OnDestroy () {
		if (_inputManager)
			_inputManager.OnMouseDown -= _inputManager_OnMouseDown;
		if (_farmManager)
			_farmManager.RecivedPonys -= _farmManager_RecivedPonys;
			
	}

	void SetState(GameState state)
	{
		if (StateManager.Instanse)
			StateManager.Instanse.SetState (state);
		else throw new NullReferenceException ("StateManager.Instanse is null");
	}

	void GenerateHerd()
	{
		_dogs = CreateObj (_sheepDogPrefab, _dogsQuantity);
		_ponys = CreateObj (_ponyPrefab, _ponyQuantity);

		foreach (var item in _ponys) {
			if (!item)
				continue;
			item.Initialize (new Vector2 {
				x = _inputManager.BottomLeftCorner.x + _filedOffset,
				y = _inputManager.BottomLeftCorner.y + _filedOffset
			},
				new Vector2 { x = _inputManager.UpRightCorner.x - _filedOffset, y = _inputManager.UpRightCorner.y - _filedOffset });
		}

		foreach (var item in _dogs) {
			if (!item)
				continue;
			item.Initialize ();
		}
	}



	private T[] CreateObj<T>(T obj, int quantity) where T:Component
	{
		if(!obj) throw new NullReferenceException (string.Format( "Prefab {0} is null", obj.GetType()));

		var collection = new T[quantity];

		for (int i = 0; i < quantity; i++) {
			var temp = obj.GetClone ();
			if (!temp)
				continue;
			temp.transform.position = RandomPosition;
			collection [i] = temp;
		}
		return collection;
	}

	private Vector2 RandomPosition
	{
		get
		{ 
			if(!_inputManager) throw new NullReferenceException ("InputManager is null");

			Vector2 bottom = _inputManager.BottomLeftCorner;
			Vector2 up = _inputManager.UpRightCorner;

			float x = UnityRandom.Range (bottom.x + _filedOffset, up.x - _filedOffset);
			float y = UnityRandom.Range (bottom.y + _filedOffset, up.y - _filedOffset);

			return new Vector2{ x = x, y = y };
		}
	}
}
