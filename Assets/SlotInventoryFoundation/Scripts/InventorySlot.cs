using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

	public InventoryItem item;

	public Image itemIcon;
	public Text itemAmount;
	public int slotIndex;

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
		if(!InventoryUI.instance.itemOnPointer){

			ItemIcon droppedItem = eventData.pointerDrag.GetComponent<ItemIcon>();
			
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

	public void OnPointerClick (PointerEventData eventData)
	{
		if(eventData.button == PointerEventData.InputButton.Right){ //Right mouse button pressed to divide stacks
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
		}else if(eventData.button == PointerEventData.InputButton.Left){ //Left mouse buttom pressed to use item or position items that are being held

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

							success = Inventory.instance.IncreaseItemAmount(slotIndex, amountToAdd);

							if(success){
								InventoryUI.instance.DecreaseItemOnPointerAmount(amountToAdd);
							}

					 	}else{
							success = Inventory.instance.IncreaseItemAmount(slotIndex, itemOnPointer.amount);

							if(success){
								InventoryUI.instance.ClearItemOnPointer();
							}
					 	}

					}else{ //If the item is different than the one that is being held
						
						InventoryUI.instance.SwapItemOnPointer(this); //Swap the item that is being held with the one on this slot
					}

				}else{ //If the slot is empty

					//Add the item being held on the slot and notifies that no item is being held
					Inventory.instance.AddItemToSlot(itemOnPointer, slotIndex);
					InventoryUI.instance.ClearItemOnPointer(); 
				}
			}
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

}
