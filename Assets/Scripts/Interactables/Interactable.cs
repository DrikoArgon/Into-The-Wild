using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

	public Transform interactableTransform;
	public float radius = 0.7f;

	void OnDrawGizmosSelected(){

		if(interactableTransform == null){
			interactableTransform = this.transform;
		}

		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(interactableTransform.position, radius);
	}

	public void CheckDistance(Transform playerTransform){
		float distance = Vector3.Distance(playerTransform.position, interactableTransform.position);

		if(distance <= radius){
			Interact();
		}
	}

	public virtual void Interact(){
		
	}
}
