using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

	public InventoryItem item;

	public Image itemIcon;
	public TextMeshProUGUI itemAmount;
	public int slotIndex;

	public bool chestSlot;

	public void AddItem(InventoryItem newItem){

		item = newItem;

		itemIcon.sprite = item.itemData.icon;
		itemIcon.enabled = true;

		itemAmount.text = item.amount.ToString();

		if(item.itemData.stackable){
			itemAmount.enabled = true;
		}else{
			itemAmount.enabled = false;
		}
	}

	public void ClearSlot(){

		item.itemData = null;
		item.amount = 0;

		itemIcon.sprite = null;
		itemIcon.enabled = false;

		itemAmount.text = "";
		itemAmount.enabled = false;
	}


	public void OnDrop (PointerEventData eventData)
	{
		Debug.Log("On drop");
		if(!InventoryUI.instance.itemOnPointer){

			ItemIcon droppedItem = eventData.pointerDrag.GetComponent<ItemIcon>();

			if(chestSlot){//If this slot is from a chest

				if(droppedItem.chestItem){ //And the item that was dropped came from the chest

					if(droppedItem.mySlot.slotIndex != slotIndex){

						if(item.itemData == null){ //This slot is empty so the item that was dropped in this slot needs to be here
							ChestInventoryManager.instance.ChangeItemSlot(droppedItem.mySlot.slotIndex, slotIndex);
						}else{
							if(droppedItem.mySlot.item.itemData.itemName == item.itemData.itemName && (droppedItem.mySlot.item.itemData.stackable && item.itemData.stackable)){
								ChestInventoryManager.instance.MergeItemStack(droppedItem.mySlot.slotIndex, slotIndex);
							}else{
								ChestInventoryManager.instance.SwapItems(droppedItem.mySlot.slotIndex, slotIndex);
							}
						}
					}

				}else{
					Debug.Log("From different inventory");
					if(item.itemData == null){ //This slot is empty so the item that was dropped in this slot needs to be removed from the inventory and added to the chest
						ChestInventoryManager.instance.AddItemToSlot(droppedItem.mySlot.item,slotIndex);
						Inventory.instance.Remove(droppedItem.mySlot.slotIndex);
					}else{//There is an item in this slot

						//It the item is stackable and is the same item, remove from the inventory and increase the amount in the chest
						if(droppedItem.mySlot.item.itemData.itemName == item.itemData.itemName && (droppedItem.mySlot.item.itemData.stackable && item.itemData.stackable)){
							Debug.Log("Same item and stackable");
							ChestInventoryManager.instance.IncreaseItemAmount(slotIndex, droppedItem.mySlot.item.amount);
							Inventory.instance.Remove(droppedItem.mySlot.slotIndex);
						}else{
							//If the item is different or is not stackable, swaps the items between the inventory and the chest
							ChestInventoryManager.instance.SwapItemsBetweenInventories(droppedItem.mySlot.slotIndex, slotIndex);
						}
					}
					
				}


			}else{ //If the slot is from the players inventory

				if(droppedItem.chestItem){

					if(item.itemData == null){ //This slot is empty so the item that was dropped in this slot needs to be removed from the inventory and added to the chest
						Inventory.instance.AddItemToSlot(droppedItem.mySlot.item,slotIndex);
						ChestInventoryManager.instance.Remove(droppedItem.mySlot.slotIndex);
					}else{//There is an item in this slot

						//It the item is stackable and is the same item, remove from the inventory and increase the amount in the chest
						if(droppedItem.mySlot.item.itemData.itemName == item.itemData.itemName && (droppedItem.mySlot.item.itemData.stackable && item.itemData.stackable)){
							Inventory.instance.IncreaseItemAmount(slotIndex, droppedItem.mySlot.item.amount);
							ChestInventoryManager.instance.Remove(droppedItem.mySlot.slotIndex);
						}else{
							//If the item is different or is not stackable, swaps the items between the inventory and the chest
							Inventory.instance.SwapItemsBetweenInventories(droppedItem.mySlot.slotIndex, slotIndex);
						}
					}

				}else{

					if(droppedItem.mySlot.slotIndex != slotIndex){

						if(item.itemData == null){ //This slot is empty so the item that was dropped in this slot needs to be here
							Inventory.instance.ChangeItemSlot(droppedItem.mySlot.slotIndex, slotIndex);
						}else{
							if(droppedItem.mySlot.item.itemData.itemName == item.itemData.itemName && (droppedItem.mySlot.item.itemData.stackable && item.itemData.stackable)){
								Inventory.instance.MergeItemStack(droppedItem.mySlot.slotIndex, slotIndex);
							}else{
								Inventory.instance.SwapItems(droppedItem.mySlot.slotIndex, slotIndex);
							}
						}
					}
				}
			}
		}
	}

	public void OnPointerClick (PointerEventData eventData)
	{

		if(eventData.button == PointerEventData.InputButton.Right){ //Right mouse button pressed to divide stacks
			HandleRightClick();
		}else if(eventData.button == PointerEventData.InputButton.Left){ //Left mouse buttom pressed to use item or position items that are being held

			HandleLeftClick();
		}
		
	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		
		if(!InventoryUI.instance.itemOnPointer && !InventoryUI.instance.itemBeingDragged){
			if(item.itemData != null){
				itemIcon.GetComponent<ItemIcon>().ActivateVisualSelectionFeedback();
				InventoryUI.instance.ActivateTooltip(item);
			}
		}
	}


	public void OnPointerExit (PointerEventData eventData)
	{

		if(item.itemData != null){
			itemIcon.GetComponent<ItemIcon>().DeactivateVisualSelectionFeedback();
			InventoryUI.instance.DeactivateTooltip();
		}
	}

	void HandleLeftClick(){

		Debug.Log("Left click");
		//If the cursor is holding an item that was divided
			if(InventoryUI.instance.itemOnPointer){ 
		
				//Get the item info
				InventoryItem itemOnPointer = InventoryUI.instance.GetCurrentStackDivisionItem();

				//If there is an item in this slot
				if(item.itemData != null){ 

					//If the item is the same as the one being held
					if(item.itemData.itemName == itemOnPointer.itemData.itemName && itemOnPointer.itemData.stackable){

					 	int newAmount = item.amount + itemOnPointer.amount;
						bool success;

					 	if(newAmount > 999){ 

							int remainingAmount = newAmount - 999;
							int amountToAdd = itemOnPointer.amount - remainingAmount;

							if(chestSlot){
								success = ChestInventoryManager.instance.IncreaseItemAmount(slotIndex, amountToAdd);
							}else{
								success = Inventory.instance.IncreaseItemAmount(slotIndex, amountToAdd);
							}

							if(success){
								InventoryUI.instance.DecreaseItemOnPointerAmount(amountToAdd);
							}


					 	}else{

							if(chestSlot){
								success = ChestInventoryManager.instance.IncreaseItemAmount(slotIndex, itemOnPointer.amount);
							}else{
								success = Inventory.instance.IncreaseItemAmount(slotIndex, itemOnPointer.amount);
							}

							if(success){
								InventoryUI.instance.ClearItemOnPointer();
							}
					 	}

					}else{ //If the item is different than the one that is being held
						
						InventoryUI.instance.SwapItemOnPointer(this); //Swap the item that is being held with the one on this slot
					}

				}else{ //If the slot is empty

					//Add the item being held on the slot and notifies that no item is being held
					if(chestSlot){
						ChestInventoryManager.instance.AddItemToSlot(itemOnPointer, slotIndex);
					}else{
						Inventory.instance.AddItemToSlot(itemOnPointer, slotIndex);
					}

					InventoryUI.instance.ClearItemOnPointer(); 
				}
			}else{
				if(item.itemData != null){
					if(ChestUI.instance.chestUI.activeSelf){
						HandleItemTransfer();
					}else{
						HandleItemUsage();
					}
				}
			}
	}


	void HandleRightClick(){

		if(item.itemData != null){ //If there is an item in this slot
			if(item.itemData.stackable){ //And the item is stackable
				if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){

					int halfAmount = item.amount / 2; 
					InventoryUI.instance.DivideStackableItem(this, halfAmount); //Remove 1 unit of the stacked item in this slot
				}else{
					InventoryUI.instance.DivideStackableItem(this, 1);
				}
			}
		}

	}

	void HandleItemUsage(){
		if(!chestSlot){
			if(item.itemData != null){

				Inventory.instance.DefineItemToBeUsedSlot(slotIndex);
				item.itemData.Use();

			}
		}
	}

	void HandleItemTransfer(){

		bool success = false;

		if(chestSlot){

			success = Inventory.instance.AddItem(item.itemData, item.amount);

			if(success){
				ChestInventoryManager.instance.Remove(slotIndex);
				InventoryUI.instance.DeactivateTooltip();
			}

		}else{

			success = ChestInventoryManager.instance.AddItem(item.itemData, item.amount);

			if(success){
				Inventory.instance.Remove(slotIndex);
				InventoryUI.instance.DeactivateTooltip();
			}
		}

		if(!success){
			Debug.Log("Inventory full");
		}

	}

}
