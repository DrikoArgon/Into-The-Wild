using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerTileSelector : MonoBehaviour {

	public static PlayerTileSelector instance;

	public Tilemap tilemap;
	public GameObject tileSelector;
	public bool toolSelected;
	public float minDistanceToGetTile;

	public FarmableTile currentFarmableTileSelected;

	private PlayerMovement playerMovement;
	private Transform tileSelectorCenter;
	// Use this for initialization
	void Awake(){
		instance = this;
	}

	void Start () {
		playerMovement = GetComponent<PlayerMovement>();
		tileSelectorCenter = tileSelector.transform.GetChild(0).transform;
	}

	public void StartTrackingPosition(){
		toolSelected = true;
		tileSelector.SetActive(true);
		StartCoroutine(PositionTileSelector());
	}

	public void StopTrackingPosition(){
		toolSelected = false;
		tileSelector.SetActive(false);
		currentFarmableTileSelected = null;
		StopAllCoroutines();
	}

	public void GetTileSelected(){
		
		RaycastHit2D hit = Physics2D.Raycast(tileSelectorCenter.transform.position, new Vector2(0.1f,0));

		if(hit.collider != null){
			currentFarmableTileSelected = hit.collider.gameObject.GetComponent<FarmableTile>();
		}else{
			currentFarmableTileSelected = null;
		}

	}

	IEnumerator PositionTileSelector(){

		while(toolSelected){

			Vector3Int playerPosition = tilemap.WorldToCell(new Vector3(transform.position.x, transform.position.y, 0));

			if(playerMovement.playerDirection == PlayerDirection.Right){
				tileSelector.transform.position = tilemap.CellToWorld(new Vector3Int(playerPosition.x + 1, playerPosition.y, playerPosition.z));

			}else if(playerMovement.playerDirection == PlayerDirection.Left){
				tileSelector.transform.position = tilemap.CellToWorld(new Vector3Int(playerPosition.x - 1, playerPosition.y, playerPosition.z));

			}else if(playerMovement.playerDirection == PlayerDirection.Up){
				tileSelector.transform.position = tilemap.CellToWorld(new Vector3Int(playerPosition.x, playerPosition.y + 1, playerPosition.z));

			}else if(playerMovement.playerDirection == PlayerDirection.Down){
				tileSelector.transform.position = tilemap.CellToWorld(new Vector3Int(playerPosition.x, playerPosition.y - 1, playerPosition.z));

			}

			GetTileSelected();

			yield return new WaitForSeconds(0.15f);
		}

		tileSelector.SetActive(false);
	}
}
