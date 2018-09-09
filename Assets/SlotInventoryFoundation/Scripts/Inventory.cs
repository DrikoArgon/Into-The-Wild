using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct InventoryItem{

	public Item itemData;
	public int amount;

	public InventoryItem(Item item, int amount){
		this.itemData = item;
		this.amount = amount;

	}

}

public class Inventory : MonoBehaviour {

	public static Inventory instance;

	public GameObject player;

	void Awake(){

		if(instance != null){
			Debug.Log("More than one inventory instance found!");
		}
		instance = this;
	}

	public delegate void OnItemChanged();

	public OnItemChanged onItemChangedCallback;

	public List<InventoryItem> items = new List<InventoryItem>();

	public int slotAmount = 30;

	public bool AddItem(Item item, int amount){

		bool success;

		if(item.stackable){

			int index = GetItemIndex(item); //Check if there is already a stack of this item that it can be inserted in

			if(index != -1){ //If there is

				success = IncreaseItemAmount(index, amount); //Tries to increase the amount or create 

			}else{

				success = AddItemToNewSlot(item, amount);
			}

		}else{

			success = AddItemToNewSlot(item, 1);

		}

		if(!success){
			return false;
		}

		if(onItemChangedCallback != null){
			onItemChangedCallback.Invoke();
		}

		return true;

	}

	public void Remove(int slotIndex){

		InventoryItem emptyItem = new InventoryItem(null, 0);

		items[slotIndex] = emptyItem;

		if(onItemChangedCallback != null){
			onItemChangedCallback.Invoke();
		}
	}

	public int GetItemIndex(Item item){

		int index = -1;

		for(int i = 0; i < items.Count; i++){

			if(items[i].itemData != null){
				if(items[i].itemData.itemName == item.itemName && items[i].amount < 999){
					index = i;
					return index;
				}
			}
		}

		return index;
	}

	public bool CheckInventorySpace(){

		foreach(InventoryItem item in items){

			if(item.itemData == null){ //If there is an element of the list without an Item, there is space in the inventory
				return true;
			}
		}

		return false;
	}

	public bool CheckInventorySpaceStackable(){

		

		foreach(InventoryItem item in items){

			if(item.itemData == null){ //If there is an element of the list without an Item, there is space in the inventory
				return true;
			}
		}

		return false;
	}

	bool AddItemToNewSlot(Item item, int amount){

		bool hasSpace = CheckInventorySpace();

		if(!hasSpace){
			Debug.Log("Inventory is full!");
			return false;
		}

		for(int i = 0; i < items.Count; i++){

			if(items[i].itemData == null){
				InventoryItem newItem = new InventoryItem(item, amount);
				items[i] = newItem;
				return true;
			}
		}

		return false;
	}

	public void AddItemToSlot(InventoryItem item, int slotIndex){

		InventoryItem newItem = new InventoryItem(item.itemData, item.amount);
		items[slotIndex] = newItem;

		if(onItemChangedCallback != null){
			onItemChangedCallback.Invoke();
		}
	}

	public bool IncreaseItemAmount(int index, int amount){

		InventoryItem newItem = items[index];

		int newAmount = items[index].amount + amount;

		if(newAmount > 999){

			int amountForNewStack = newAmount - 999; //Ex: Picked up 5 units of an item and already had 997 -> newAmount = 997 + 5 = 1002 1002 - 999 = 3 <- this is the amount of items that needs to be added to a new stack
			int amountToAdd = amount - amountForNewStack;// 5 - 3 = 2 <- this is the amount that will be added to the already existing stack

			bool success = AddItemToNewSlot(newItem.itemData, amountForNewStack);

			if(success){
				newItem.amount += amountToAdd;
				items[index] = newItem;

				if(onItemChangedCallback != null){
					onItemChangedCallback.Invoke();
				}

				return true;

			}else{

				return false;
			}


		}else{

			newItem.amount += amount;
			items[index] = newItem;

			if(onItemChangedCallback != null){
				onItemChangedCallback.Invoke();
			}

			return true;
		}



	}

	public void DecreaseItemAmount(int index, int amount){

		InventoryItem newItem = items[index];
		newItem.amount -= amount;

		if(newItem.amount == 0){
			Remove(index);
		}else{
			items[index] = newItem;

			if(onItemChangedCallback != null){
				onItemChangedCallback.Invoke();
			}
		}

	}

	public void ChangeItemSlot(int originalSlotIndex, int newSlotIndex){

		InventoryItem emptyItem = new InventoryItem(null, 0);

		items[newSlotIndex] = items[originalSlotIndex];

		items[originalSlotIndex] = emptyItem;

		if(onItemChangedCallback != null){
			onItemChangedCallback.Invoke();
		}
	}

	public void MergeItemStack(int originalSlotIndex, int newSlotIndex){

		InventoryItem emptyItem = new InventoryItem(null, 0);

		InventoryItem tempItem = items[newSlotIndex];

		int newAmount = tempItem.amount + items[originalSlotIndex].amount;

		if(newAmount > 999){

			int remainingAmount = newAmount - 999;
			int amountToAdd = items[originalSlotIndex].amount - remainingAmount;

			if(amountToAdd == 0){

				SwapItems(originalSlotIndex, newSlotIndex);
				return;

			}else{
				tempItem.amount += amountToAdd;
				InventoryUI.instance.InsertItemOnPointer(new InventoryItem(items[originalSlotIndex].itemData, remainingAmount));
			}

		}else{

			tempItem.amount += items[originalSlotIndex].amount;
		}

		items[newSlotIndex] = tempItem;
		items[originalSlotIndex] = emptyItem;

		if(onItemChangedCallback != null){
			onItemChangedCallback.Invoke();
		}
	}

	public void SwapItems(int originalSlotIndex, int newSlotIndex){

		InventoryItem swappedItem = items[newSlotIndex];

		items[newSlotIndex] = items[originalSlotIndex];

		items[originalSlotIndex] = swappedItem;

		if(onItemChangedCallback != null){
			onItemChangedCallback.Invoke();
		}
	}

	public void DropItem(int slotIndex){

		GameObject droppedItem = Instantiate(items[slotIndex].itemData.pickUpPrefab, player.transform.position, Quaternion.identity);

		droppedItem.GetComponent<ItemPickup>().amount = items[slotIndex].amount;

		Remove(slotIndex);

	}

	public void DropItemOnPointer(){

		GameObject droppedItem = Instantiate(InventoryUI.instance.GetCurrentStackDivisionItem().itemData.pickUpPrefab, player.transform.position, Quaternion.identity);

		droppedItem.GetComponent<ItemPickup>().amount = InventoryUI.instance.GetCurrentStackDivisionItem().amount;

		InventoryUI.instance.ClearItemOnPointer();
	}
}
