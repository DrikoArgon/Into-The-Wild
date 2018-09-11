using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerInteractionControls : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(EventSystem.current.IsPointerOverGameObject()){
			return;
		}

		if(GameManager.instance.uiActive){
			return;
		}

		if(Input.GetMouseButtonDown(1)){
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

			if(hit.collider != null){
				Interactable interactable = hit.collider.GetComponent<Interactable>();

				if(interactable != null){
					interactable.CheckDistance(this.transform); 
				}
			}
		}
	}
}
