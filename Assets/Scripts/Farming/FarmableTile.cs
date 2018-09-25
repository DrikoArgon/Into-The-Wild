using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmableTile : MonoBehaviour {

	public int tileIndex;

	public bool tilled;
	public bool watered;
	public bool seeded;
	public int seedIndex = -1;
	public CropState cropState;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PrepareSoil(){
		if(!tilled){
			tilled = true;
			FarmableTileManager.instance.InsertTilledTile(tileIndex);
		}
	}

	public void ReturnToNormalSoil(){
		tilled = false;
		FarmableTileManager.instance.RemoveTilledTile(tileIndex);
	}

	public void WaterSoil(){
		if(!watered && tilled){
			watered = true;
			FarmableTileManager.instance.InsertWateredTile(tileIndex);
		}
	}

	public void DrySoil(){
		watered = false;
		FarmableTileManager.instance.InsertTilledTile(tileIndex);
	}
}

public enum CropState{
	Seed,
	Sprout,
	Grown,
	Flower,
	Harvestable
}
