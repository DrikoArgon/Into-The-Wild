using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	private Vector3 screenPoint;
	private Vector3 mousePosition;

	public InventorySlot mySlot;

	public void Start(){
		mySlot = GetComponentInParent<InventorySlot>();
	}

	public void OnBeginDrag (PointerEventData eventData)
	{
		if(!InventoryUI.instance.itemOnPointer){
			if(GetComponent<Image>().enabled){ //If there's an item 

				InventoryUI.instance.itemBeingDragged = true;
				DeactivateVisualSelectionFeedback();

				screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
				mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, screenPoint.z));

				this.transform.localPosition = mousePosition;
				this.transform.SetParent(InventoryUI.instance.inventoryUI.transform, false);
				GetComponent<Image>().raycastTarget = false;
			}
		}
	}

	public void OnDrag (PointerEventData eventData)
	{
		if(!InventoryUI.instance.itemOnPointer){
			mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, screenPoint.z));
			this.transform.position = mousePosition;
		}
	}

	public void OnEndDrag (PointerEventData eventData)
	{

		InventoryUI.instance.itemBeingDragged = false;
		this.transform.SetParent(mySlot.transform.GetChild(0).transform, false);
		this.transform.localPosition = Vector3.zero;
		GetComponent<Image>().raycastTarget = true;
		
	}

	public void ActivateVisualSelectionFeedback(){
		transform.localScale = new Vector3(1.1f, 1.1f, 1);
	}

	public void DeactivateVisualSelectionFeedback(){
		transform.localScale = new Vector3(1, 1, 1);
	}

}
