using UnityEngine.UI;
using UnityEngine;

public class InventoryUI : MonoBehaviour {

	public static InventoryUI instance;

	public Transform itemsParent;

	InventorySlot[] slots;

	Inventory inventory;

	public GameObject inventoryUI;
	public GameObject tooltip;
	public GameObject stackedItemDivisionPrefab;
	public StackableItemDivision currentStackedItemDivision;

	private Vector3 screenPoint;
	private Vector3 mousePosition;
	public bool itemOnPointer;
	public bool itemBeingDragged;


	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
		inventory = Inventory.instance;
		inventory.onItemChangedCallback += UpdateUI;

		slots = itemsParent.GetComponentsInChildren<InventorySlot>();

		DefineSlotIndexes();

		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Inventory") && !itemBeingDragged && !itemOnPointer){
			inventoryUI.SetActive(!inventoryUI.activeSelf);
		}

		if(itemOnPointer && currentStackedItemDivision != null){
			DeactivateTooltip(); 
			mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)); 
			currentStackedItemDivision.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
		}

	}

	void DefineSlotIndexes(){

		for(int i = 0; i < slots.Length; i++){
			slots[i].slotIndex = i;
		}

	}

	void UpdateUI(){

		for(int i = 0; i < slots.Length; i++){

			if(inventory.items[i].itemData != null){
				slots[i].AddItem(inventory.items[i]);
			}else{
			   slots[i].ClearSlot();
			}

		}

	}

	public void InsertItemOnPointer(InventoryItem item){

		//Get the mouse position

		mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)); 

		itemOnPointer = true;

		//Create the object that will follow the mouse. This object contains the item's icon and amount
		currentStackedItemDivision = Instantiate(stackedItemDivisionPrefab, inventoryUI.transform).GetComponent<StackableItemDivision>();

		//Sets the item on the new object that will follow the mouse
		currentStackedItemDivision.SetItem(item); 

		currentStackedItemDivision.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
	}

	public void DivideStackableItem(InventorySlot originalSlot, int amount){
		
		if(!itemOnPointer){ //Divide the stack for the first time

			InsertItemOnPointer(new InventoryItem(originalSlot.item.itemData, amount));

			Inventory.instance.DecreaseItemAmount(originalSlot.slotIndex, amount);

		}else{

			if(currentStackedItemDivision != null){
				if(currentStackedItemDivision.item.itemData.itemName == originalSlot.item.itemData.itemName){

					Inventory.instance.DecreaseItemAmount(originalSlot.slotIndex, amount);
					currentStackedItemDivision.IncreaseAmount(amount);

				}
			}

		}
	}

	public void SwapItemOnPointer(InventorySlot slot){

		InventoryItem slotItem = slot.item;

		Inventory.instance.AddItemToSlot(currentStackedItemDivision.item, slot.slotIndex);
		currentStackedItemDivision.SetItem(slotItem);

	}

	public void DecreaseItemOnPointerAmount(int amount){

		currentStackedItemDivision.DecreaseAmount(amount);
	}

	public void ClearItemOnPointer(){

		Destroy(currentStackedItemDivision.gameObject);
		currentStackedItemDivision = null;
		itemOnPointer = false;

	}

	public InventoryItem GetCurrentStackDivisionItem(){

		return currentStackedItemDivision.item;
		
	}

	public void ActivateTooltip(InventoryItem item){
		tooltip.GetComponent<InventoryTooltip>().Activate(item);
	}

	public void DeactivateTooltip(){
		tooltip.GetComponent<InventoryTooltip>().Deactivate();
	}
}
