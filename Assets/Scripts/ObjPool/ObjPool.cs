using UnityEngine;
using System.Collections.Generic;

public class ObjPool
{
	#region Singletone region

	private static ObjPool _instance;

	public static ObjPool Instance {

		get { 
			if (_instance == null)
				_instance = new ObjPool ();
			return _instance;
		}
	}

	private ObjPool ()
	{

	}

	#endregion

	private Dictionary<Component,List<Component>> _pool = new Dictionary<Component,List<Component>> ();

	public T GetClone<T>(T prefab) where T : Component
	{
		if (_pool.ContainsKey (prefab)) 
		{
			var tempList = _pool [prefab];
			if (tempList == null) {
				tempList = new List<Component> ();
			}

//			if(tempList.Count == 0){
//				var tempItem = MonoBehaviour.Instantiate (prefab) as GameObject;
//				tempList.Add (tempItem);
//				return tempItem;
//			}

			foreach (var item in tempList) {
				if (!item)
					continue;
				if (!item.gameObject.activeSelf) {
					item.gameObject.SetActive (true);
					return(item as T);
				}
			}

			var tempItem = MonoBehaviour.Instantiate (prefab) as T;
			tempList.Add (tempItem);
			return tempItem;



		} else {
			_pool.Add (prefab, new List<Component> ());
			return GetClone<T>(prefab);
		}
	}
}

public static class PoolExtensions
{
	public static T GetClone<T>(this T obj) where T : Component
	{
		if (!obj)
			return null;
		return ObjPool.Instance.GetClone<T> (obj);
	}

	public static void PutClone (this Component obj)
	{
		if (!obj)
			return;
		obj.gameObject.SetActive (false);
	}
}
