using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {

	public GameObject[] itemPrefabs;
	public bool burstSpawn;
	public int burstAmount = 10;

	// Use this for initialization
	void Start () {
		if(burstSpawn){
			StartCoroutine(BurstSpawn());
		}else{
			StartCoroutine(SpawnItem());
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator SpawnItem(){

		if(itemPrefabs.Length > 0){
			int randomIndex = Random.Range(0, itemPrefabs.Length);

			Instantiate(itemPrefabs[randomIndex], transform.position, Quaternion.identity);

			yield return new WaitForSeconds(1.5f);

			StartCoroutine(SpawnItem());
		}else{
			yield break;
		}
	}

	IEnumerator BurstSpawn(){

		yield return new WaitForSeconds(2f);

		if(itemPrefabs.Length > 0){
			

			for(int i = 0; i < burstAmount; i++){
				int randomIndex = Random.Range(0, itemPrefabs.Length);
				Instantiate(itemPrefabs[randomIndex], transform.position, Quaternion.identity);
			}

			yield return new WaitForSeconds(3f);

			StartCoroutine(BurstSpawn());
		}else{
			yield break;
		}
	}
}
