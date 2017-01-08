using System;
using UnityEngine;
using System.Collections;
using UnityRandom = UnityEngine.Random;
using PonyGame.Data;


public class Pony : MonoBehaviour {

	public bool IsInFarm {
		get;
		set;
	}

	[SerializeField]
	private Animator _animator;

	[SerializeField, Range (.1f,.3f)]
	private float _movingSpeed = .1f;

	[SerializeField]
	private float _distance=.5f;

	[SerializeField]
	private float _folowDogSpeed =1f;

	[SerializeField]
	private Collider2D _collider;

	private float _moveTime =1f;
	private Vector2 _currentDirection;
	private float _timeToSwitch;
	private Vector2 _upCorner;
	private Vector2 _bottomCorner;
	private Transform _leader;
	private SheepDog _currentDog;
	private Vector2 _farmPlace;

	public Vector2 RandomDirection {
		get
		{ 
			int val = UnityRandom.Range (0, 4);
			switch (val) {
			case 0:
				return Vector2.right;
			case 1:
				return Vector2.left;
			case 2:
				return Vector2.up;
			case 3:
				return Vector2.down;
			default:
				throw new ArgumentOutOfRangeException(string.Format ("_dogs is null"));
				break;
			}
		}
	}

	private void Start () {
	
	}
	
	private void Update () {
		if (IsInFarm) {
			GoToFarmPlaceHandler ();
			return;
		}

		if (!_leader)
			FreeMovementHandler ();
		else
			FollowTheLeaderHanler ();



	}
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (_leader || IsInFarm)
			return;
		var tempShepDog = other.gameObject.GetComponent<SheepDog> ();
		if (!tempShepDog)
			return;

		_leader = tempShepDog.AddPonyInLine (this);;
		_currentDog = tempShepDog;
		if (_leader && _collider)
			_collider.enabled = false;
	}

	public void Initialize(Vector2 bottom, Vector2 up)
		{
		_upCorner = up;
		_bottomCorner = bottom;

		if (_collider)
			_collider.enabled = true;
		_leader = null;
		_currentDog = null;
		IsInFarm = false;
		}



	private void FreeMovementHandler()
		{
		if (Time.time > _timeToSwitch) 
			{
			_moveTime = UnityRandom.Range (GameConfig.MinPonyMoveTime, GameConfig.MaxPonyMoveTime);
			_currentDirection = RandomDirection;
			_timeToSwitch = Time.time + _moveTime;
			}

		transform.Translate (_currentDirection * Time.deltaTime * _movingSpeed);
		transform.position = ClampInFieldPos (transform.position);

		UpdateAnimation (_currentDirection);
		}

	private Vector2 ClampInFieldPos(Vector2 pos)
		{
		float x = Mathf.Clamp (pos.x, _bottomCorner.x, _upCorner.x);
		float y = Mathf.Clamp (pos.y, _bottomCorner.x, _upCorner.y);

		if (x != pos.x || y != pos.y)
			_timeToSwitch = 0;

		return new Vector2{ x = x, y = y };
		}

	private void UpdateAnimation(Vector2 dir)
		{
		if (_animator) {
			_animator.SetFloat (GameConfig.HorizontalKey, dir.x);
			_animator.SetFloat (GameConfig.VerticalKey, dir.y);
		}
	}
	private void FollowTheLeaderHanler()
	{
		if (!_leader)
			return;
		if (Vector2.Distance (transform.position, _leader.position) < _distance)
			return;
		transform.position = Vector3.Lerp (transform.position, _leader.position, Time.deltaTime * _folowDogSpeed);

		if(!_currentDog) throw new NullReferenceException ("_currentDog is not asigned.");

		_currentDirection = _leader.transform.position - transform.position;

		UpdateAnimation (_currentDirection.normalized);
	}
	public void TakePlaceInFarmHouse(Vector2 point)
	{
		_farmPlace = point;
		_leader = null;
		_currentDog = null;
		IsInFarm = true;
	}
	private void GoToFarmPlaceHandler()
	{
		transform.position = Vector3.MoveTowards (transform.position, _farmPlace, Time.deltaTime * _folowDogSpeed);
		Vector2 dir = _farmPlace - (Vector2)transform.position;
		UpdateAnimation (dir);
	}
}
