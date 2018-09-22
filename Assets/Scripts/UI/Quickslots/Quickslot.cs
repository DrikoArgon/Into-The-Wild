using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Quickslot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public Image itemIcon;
	public InventoryItem item;
	public TextMeshProUGUI itemAmount;
	public TextMeshProUGUI quickSlotButtonName;

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

	public void OnPointerEnter (PointerEventData eventData)
	{
		
		if(!InventoryUI.instance.inventoryUI.activeSelf){
			if(item.itemData != null){
				QuickslotManager.instance.ActivateTooltip(item);
			}
		}
	}

	public void OnPointerExit (PointerEventData eventData)
	{

		if(item.itemData != null){
			QuickslotManager.instance.DeactivateTooltip();
		}
	}
}
