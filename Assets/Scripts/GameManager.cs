using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public bool uiActive;

	void Awake(){


		if(GameManager.instance == null){
			instance = this;
			DontDestroyOnLoad(this);
		}else{
			Destroy(gameObject);
		}

	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
