using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickslotManager : MonoBehaviour {

	public static QuickslotManager instance;

	public GameObject slotsPanel;
	public GameObject selector;
	Quickslot[] slots;
	public GameObject tooltip;

	private Inventory inventory;
	public int currentIndexSelected;

	public KeyCode[] quickslotButtons;

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
		inventory = Inventory.instance;
		inventory.onItemChangedCallback += UpdateUI;
		currentIndexSelected = 0;
		slots = slotsPanel.GetComponentsInChildren<Quickslot>();

		UpdateUI();
		DefineQuickslotButtonsUI();

	}

	void Update(){

		for (int i = 0; i < quickslotButtons.Length; i++) {
			if(Input.GetKeyDown(quickslotButtons[i])){
				MoveSelector(i);
				break;
			}
		}


		if(Input.GetAxis("MouseScrollWheel") > 0){
			MoveSelector(currentIndexSelected + 1);	
		}else if(Input.GetAxis("MouseScrollWheel") < 0){
			MoveSelector(currentIndexSelected - 1);
		}

	}
	
	// Update is called once per frame
	void UpdateUI () {

		for(int i = 0; i < slots.Length; i++){

			if(inventory.items[i].itemData != null){
				slots[i].AddItem(inventory.items[i]);
			}else{
			   slots[i].ClearSlot();
			}

		}

		UpdateItemSelected();
	}

	public void DefineQuickslotButtonsUI(){

		for (int i = 0; i < slots.Length; i++) {

			if(quickslotButtons[i].ToString().Contains("Keypad")){
		
				char correctName = quickslotButtons[i].ToString()[6];
				slots[i].quickSlotButtonName.text = correctName.ToString();

			}else if(quickslotButtons[i].ToString().Contains("Alpha")){

				char correctName = quickslotButtons[i].ToString()[5];
				slots[i].quickSlotButtonName.text = correctName.ToString();

			}else{
				slots[i].quickSlotButtonName.text = quickslotButtons[i].ToString();
			}
		}

	}

	void MoveSelector(int index){

		if(index < 0){
			index = 0;
		}

		if(index >= slots.Length){
			index = slots.Length - 1;
		}

		currentIndexSelected = index;
		selector.transform.position = slots[index].transform.position;

		UpdateItemSelected();			
	}

	void UpdateItemSelected(){

		if(slots[currentIndexSelected].item.itemData != null){
			PlayerInteractionControls.instance.SetCurrentItem(slots[currentIndexSelected].item.itemData); 
		}else{
			PlayerInteractionControls.instance.ClearItem();
		}
	}

	public void ActivateTooltip(InventoryItem item){
		tooltip.GetComponent<InventoryTooltip>().Activate(item);
	}

	public void DeactivateTooltip(){
		tooltip.GetComponent<InventoryTooltip>().Deactivate();
	}

}
