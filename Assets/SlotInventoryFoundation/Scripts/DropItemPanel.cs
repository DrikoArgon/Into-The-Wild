using UnityEngine.EventSystems;
using UnityEngine;

public class DropItemPanel : MonoBehaviour, IDropHandler, IPointerClickHandler {


	public void OnDrop (PointerEventData eventData)
	{
		if(!InventoryUI.instance.itemOnPointer){
			ItemIcon droppedItem = eventData.pointerDrag.GetComponent<ItemIcon>();

			Inventory.instance.DropItem(droppedItem.mySlot.slotIndex);
		}
	}

	public void OnPointerClick (PointerEventData eventData)
	{
		if(InventoryUI.instance.itemOnPointer){
			if(eventData.button == PointerEventData.InputButton.Left){
				
				Inventory.instance.DropItemOnPointer();
			}
		}
	}
}
