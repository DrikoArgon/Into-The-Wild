using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public bool uiActive;

	public string playerName;

	public bool gamepadMode;

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
		playerName = "Ryan";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
