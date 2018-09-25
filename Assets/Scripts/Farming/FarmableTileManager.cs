using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FarmableTileManager : MonoBehaviour {

	public static FarmableTileManager instance;

	public GameObject farmableTilesHolder;
	public FarmableTile[] farmableTiles;

	public Tilemap tilledSoilTillemap;

	public RuleTile tilledSoilTile;
	public RuleTile wateredSoilTile;

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
		GetAllFarmableTiles();
		DefineFarmableTileIndexes();
	}

	void GetAllFarmableTiles(){
		farmableTiles = farmableTilesHolder.GetComponentsInChildren<FarmableTile>();
	}

	void DefineFarmableTileIndexes(){
		for (int i = 0; i < farmableTiles.Length; i++) {
			farmableTiles[i].tileIndex = i;
		}
	}

	public void DryAllSoil(){

		for (int i = 0; i < farmableTiles.Length; i++) {
			if(farmableTiles[i].tilled){
				farmableTiles[i].DrySoil();
			}
		}

	}

	public void InsertTilledTile(int index){

		Vector3Int farmableTileCellPosition = tilledSoilTillemap.WorldToCell(new Vector3(farmableTiles[index].transform.position.x, farmableTiles[index].transform.position.y, 0));

		tilledSoilTillemap.SetTile(farmableTileCellPosition, tilledSoilTile);

	}

	public void RemoveTilledTile(int index){

		Vector3Int farmableTileCellPosition = tilledSoilTillemap.WorldToCell(new Vector3(farmableTiles[index].transform.position.x, farmableTiles[index].transform.position.y, 0));

		tilledSoilTillemap.SetTile(farmableTileCellPosition, null);

	}


	public void InsertWateredTile(int index){

		Vector3Int farmableTileCellPosition = tilledSoilTillemap.WorldToCell(new Vector3(farmableTiles[index].transform.position.x, farmableTiles[index].transform.position.y, 0));

		tilledSoilTillemap.SetTile(farmableTileCellPosition, wateredSoilTile);
	}

	public void RemoveWateredTile(int index){

		Vector3Int farmableTileCellPosition = tilledSoilTillemap.WorldToCell(new Vector3(farmableTiles[index].transform.position.x, farmableTiles[index].transform.position.y, 0));

		tilledSoilTillemap.SetTile(farmableTileCellPosition, null);

	}
}
