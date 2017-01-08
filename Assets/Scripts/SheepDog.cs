using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PonyGame.Data;

using System;

public class SheepDog : MonoBehaviour {
	[SerializeField]
	private SpriteRenderer _renderer;

	[SerializeField]
	private Color _selectColor;

	[SerializeField]
	private float _speed = 1f;

	private Color _originColor;
	private Vector2 _targetPoint;
	private List<Pony> _ponys = new List<Pony> ();
	private float _originSpeed;

	public float Speed {
		get
		{ 
			return _speed;
		}
		set
		{ 
			_speed = value;
		}
	}

	public List<Pony> Ponys
	{
		get
		{ 
			return _ponys;
		}
	}

	private void Start () {
		_originColor = _renderer.material.color;
		_targetPoint = transform.position;
		_originSpeed = _speed;
	}
	
	private void LateUpdate () {
		transform.position = Vector2.MoveTowards (transform.position, _targetPoint, Time.deltaTime * _speed);
	}

	public void Initialize()
	{
		_ponys.Clear ();
		if (_originSpeed != 0)
			_speed = _originSpeed;
	}

	public Transform AddPonyInLine(Pony pony)
	{
		if (!pony)
			return null;
		var last = GetNerestObj (pony.transform.position);
			_ponys.Add (pony);
		return last;
	}

	public void ResetPonysColection()
	{
		_ponys.Clear();
	}

	public void Select()
	{
		if(!_renderer)throw new NullReferenceException ("_render is null");
		_renderer.material.color = _selectColor;
	}

	public void UnSelect()
	{
		if(!_renderer)throw new NullReferenceException ("_render is null");
		_renderer.material.color = _originColor;
	}

	public void MoveToPoint(Vector2 point)
	{
		_targetPoint = point;
		bool condition = transform.position.x > point.x;
		if (_renderer)
			_renderer.flipX = condition;
	}

	public void StopMoving()
	{
		_targetPoint = transform.position;
	}

	private Transform GetNerestObj(Vector2 point)
	{
		Transform minDistanceTransform = null;

		foreach (var item in _ponys) {
			if (!item)
				continue;
			if (minDistanceTransform == null || (Vector2.Distance (point, item.transform.position) < Vector2.Distance (point, minDistanceTransform.position)))
				minDistanceTransform = item.transform;
		}
		return minDistanceTransform == null ? transform : minDistanceTransform;
	}
}
