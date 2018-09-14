using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInventoryManager : MonoBehaviour {

	public static ChestInventoryManager instance;

	public GameObject player;

	public Chest currentChest;

	void Awake(){

		if(instance != null){
			Debug.Log("More than one chest inventory manager instance found!");
		}
		instance = this;
	}

	public delegate void OnChestItemChanged();

	public OnChestItemChanged onChestItemChangedCallback;

	public List<InventoryItem> items = new List<InventoryItem>();

	public int slotAmount = 30;

	public void SetupChestInventory(List<InventoryItem> chestInv, Chest chest){

		currentChest = chest;

		for(int i = 0; i < slotAmount; i++){

			items[i] = chestInv[i];
		}

		if(onChestItemChangedCallback != null){
			onChestItemChangedCallback.Invoke();
		}
	}

	public void UpdateChestItems(){

		if(currentChest != null){
			Debug.Log("Updating");
			currentChest.UpdateChestItems(items);
		}
	}

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

		if(onChestItemChangedCallback != null){
			onChestItemChangedCallback.Invoke();
		}

		return true;

	}

	public void Remove(int slotIndex){

		InventoryItem emptyItem = new InventoryItem(null, 0);

		items[slotIndex] = emptyItem;

		if(onChestItemChangedCallback != null){
			onChestItemChangedCallback.Invoke();
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

	public bool CheckChestInventorySpace(){

		foreach(InventoryItem item in items){

			if(item.itemData == null){ //If there is an element of the list without an Item, there is space in the inventory
				return true;
			}
		}

		return false;
	}


	bool AddItemToNewSlot(Item item, int amount){

		bool hasSpace = CheckChestInventorySpace();

		if(!hasSpace){
			Debug.Log("Chest is full!");
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

		if(onChestItemChangedCallback != null){
			onChestItemChangedCallback.Invoke();
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

				if(onChestItemChangedCallback != null){
					onChestItemChangedCallback.Invoke();
				}

				return true;

			}else{

				return false;
			}


		}else{

			newItem.amount += amount;
			items[index] = newItem;

			if(onChestItemChangedCallback != null){
				onChestItemChangedCallback.Invoke();
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

			if(onChestItemChangedCallback != null){
				onChestItemChangedCallback.Invoke();
			}
		}

	}

	public void ChangeItemSlot(int originalSlotIndex, int newSlotIndex){

		InventoryItem emptyItem = new InventoryItem(null, 0);

		items[newSlotIndex] = items[originalSlotIndex];

		items[originalSlotIndex] = emptyItem;

		if(onChestItemChangedCallback != null){
			onChestItemChangedCallback.Invoke();
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

		if(onChestItemChangedCallback != null){
			onChestItemChangedCallback.Invoke();
		}
	}

	public void SwapItems(int originalSlotIndex, int newSlotIndex){

		InventoryItem swappedItem = items[newSlotIndex];

		items[newSlotIndex] = items[originalSlotIndex];

		items[originalSlotIndex] = swappedItem;

		if(onChestItemChangedCallback != null){
			onChestItemChangedCallback.Invoke();
		}
	}

	public void SwapItemsBetweenInventories(int inventoryItemIndex, int chestItemIndex){

		InventoryItem chestItem = items[chestItemIndex];
		InventoryItem inventoryItem = Inventory.instance.items[inventoryItemIndex];

		items[chestItemIndex] = inventoryItem;

		Inventory.instance.Remove(inventoryItemIndex);

		Inventory.instance.AddItemToSlot(chestItem, inventoryItemIndex);

		if(onChestItemChangedCallback != null){
			onChestItemChangedCallback.Invoke();
		}

	}

	public void DropItem(int slotIndex){

		GameObject droppedItem = Instantiate(items[slotIndex].itemData.pickUpPrefab, player.transform.position, Quaternion.identity);
		droppedItem.GetComponent<ItemPickup>().amount = items[slotIndex].amount;
		droppedItem.GetComponent<ItemPickup>().SetAsDroppedItem();

		Remove(slotIndex);

	}

	public void DropItemOnPointer(){

		GameObject droppedItem = Instantiate(InventoryUI.instance.GetCurrentStackDivisionItem().itemData.pickUpPrefab, player.transform.position, Quaternion.identity);

		droppedItem.GetComponent<ItemPickup>().amount = InventoryUI.instance.GetCurrentStackDivisionItem().amount;

		droppedItem.GetComponent<ItemPickup>().SetAsDroppedItem();

		InventoryUI.instance.ClearItemOnPointer();
	}


}
