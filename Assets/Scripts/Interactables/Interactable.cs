using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

	public Sprite interactableIcon;

	public virtual void Interact(){
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player"){
			PlayerInteractionControls.instance.SetCurrentInteractable(this);
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if(other.tag == "Player"){
			PlayerInteractionControls.instance.ClearInteractable();
		}
	}
}
