using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable {

	public List<InventoryItem> chestItems;
	// Use this for initialization
	public int slotAmount = 30;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void Interact ()
	{
		ChestInventoryManager.instance.SetupChestInventory(chestItems, this);
		ChestUI.instance.OpenChestInventory();
	}

	public void UpdateChestItems(List<InventoryItem> items){

		for(int i = 0; i < slotAmount; i++){

			chestItems[i] = items[i];
		}

	}

	void InitializeChestItems(){

		for(int i = 0; i < slotAmount; i++){

			InventoryItem emptyItem = new InventoryItem(null, 0);

			chestItems.Add(emptyItem);
		}

	}
}
