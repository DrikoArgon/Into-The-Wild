using UnityEngine;
using UnityEngine.UI;

public class PlayerActionsUI : MonoBehaviour {

	public Image interactableButtonIcon;
	public Image useButtonIcon;


	void Start(){
		PlayerInteractionControls.instance.onItemChangedCallback += UpdateItemUI;
		PlayerInteractionControls.instance.onInteractableChangedCallback += UpdateInteractableUI;

		UpdateItemUI();
		UpdateInteractableUI();

	}
	// Update is called once per frame
	void UpdateInteractableUI() {

		if(PlayerInteractionControls.instance.currentInteractableSelected != null){
			interactableButtonIcon.enabled = true;
			interactableButtonIcon.sprite = PlayerInteractionControls.instance.currentInteractableSelected.interactableIcon;
		}else{
			interactableButtonIcon.enabled = false;
		}
	}

	void UpdateItemUI(){

		if(PlayerInteractionControls.instance.currentItemSelected != null){
			useButtonIcon.sprite = PlayerInteractionControls.instance.currentItemSelected.icon;
			useButtonIcon.enabled = true;
		}else{
			useButtonIcon.enabled = false;
		}
	}
}
