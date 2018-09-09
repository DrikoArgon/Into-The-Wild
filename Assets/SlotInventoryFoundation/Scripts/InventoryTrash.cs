using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryTrash : MonoBehaviour, IDropHandler, IPointerClickHandler {

	public void OnDrop (PointerEventData eventData)
	{
		if(!InventoryUI.instance.itemOnPointer){
			ItemIcon droppedItem = eventData.pointerDrag.GetComponent<ItemIcon>();

			Inventory.instance.Remove(droppedItem.mySlot.slotIndex);
		}

	}

	public void OnPointerClick (PointerEventData eventData)
	{
		if(InventoryUI.instance.itemOnPointer){
			if(eventData.button == PointerEventData.InputButton.Left){
				InventoryUI.instance.ClearItemOnPointer();
			}
		}
	}

}
