using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class InputManager : MonoBehaviour {
	[SerializeField]
	private Camera _mainSceneCamera;

	public event Action <RaycastHit2D> OnMouseDown;

	private void OnMouseDownHandler(RaycastHit2D data)
	{
		if (OnMouseDown != null)
			OnMouseDown (data);
	}

	public Vector2 UpRightCorner
	{
		get
		{ 
			if (!_mainSceneCamera)
				throw new NullReferenceException ("Main camera is null.");
			return _mainSceneCamera.ScreenToWorldPoint (new Vector2{x=Screen.width,y=Screen.height });
		}
	}

	public Vector2 BottomLeftCorner
	{
		get
		{ 
			if (!_mainSceneCamera)
				throw new NullReferenceException ("Main camera is null.");
			return _mainSceneCamera.ScreenToWorldPoint (Vector2.zero);
		}
	}
	
	void LateUpdate () {
		if (!_mainSceneCamera)
			throw new NullReferenceException ("Main camera is null.");

		if (Input.GetKeyDown (KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject ()) {
			Vector2 point = _mainSceneCamera.ScreenToWorldPoint (Input.mousePosition);
			var hit = Physics2D.Raycast (point, Vector2.zero);
			if (!hit.collider)
				hit.point = point;
			OnMouseDownHandler (hit);
		}
	}
}
