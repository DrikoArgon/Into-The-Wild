using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerInteractionControls : MonoBehaviour {

	public static PlayerInteractionControls instance;

	public Item currentItemSelected;
	public Interactable currentInteractableSelected;

	public delegate void OnInteractableChanged();
	public OnInteractableChanged onInteractableChangedCallback;

	public delegate void OnItemChanged();
	public OnItemChanged onItemChangedCallback;

	void Awake(){
		instance = this;
	}

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
			if(currentInteractableSelected != null){
				currentInteractableSelected.Interact();
			}
		}

		if(Input.GetMouseButtonDown(0)){
			if(currentItemSelected != null){
				Inventory.instance.DefineItemToBeUsedSlot(QuickslotManager.instance.currentIndexSelected);
				currentItemSelected.Use();
			}
		}
	}

	public void SetCurrentItem(Item item){
		currentItemSelected = item;

		if(onItemChangedCallback != null){
			onItemChangedCallback.Invoke();
		}
	}

	public void SetCurrentInteractable(Interactable interactable){
		currentInteractableSelected = interactable;

		if(onInteractableChangedCallback != null){
			onInteractableChangedCallback.Invoke();
		}
	}

	public void ClearInteractable(){
		currentInteractableSelected = null;

		if(onInteractableChangedCallback != null){
			onInteractableChangedCallback.Invoke();
		}
	}

	public void ClearItem(){
		currentItemSelected = null;

		if(onItemChangedCallback != null){
			onItemChangedCallback.Invoke();
		}
	}
}
