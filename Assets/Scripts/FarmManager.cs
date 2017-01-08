using UnityEngine;
using System;
using UnityRandom = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;

public class FarmManager : MonoBehaviour {

	public event Action<int> RecivedPonys;
	private void RecivedPonysHandler(int quantity)
	{
		if (RecivedPonys != null)
			RecivedPonys (quantity);
	}

	// Use this for initialization
	void OnTriggerEnter2D (Collider2D other) {
		var tempSheepDog = other.gameObject.GetComponent<SheepDog> ();
		if (!tempSheepDog)
			return;
		AddToFarmPony (tempSheepDog.Ponys);
		tempSheepDog.ResetPonysColection ();
	}
	
	// Update is called once per frame
	void AddToFarmPony (List<Pony> _ponys) {
		if (_ponys == null || _ponys.Count == 0)
			return;
		
		int counter =0;

		foreach (var item in _ponys) {
			if(!item)continue;
			item.TakePlaceInFarmHouse(RandomPointInFarm);
			counter++;
		}
		RecivedPonysHandler(counter);
	}

	private Vector2 RandomPointInFarm
	{
		get
		{
			return(Vector2)transform.position+UnityRandom.insideUnitCircle;
		}
	}
}
