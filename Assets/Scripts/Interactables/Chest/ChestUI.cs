using UnityEngine.UI;
using UnityEngine;

public class ChestUI : MonoBehaviour {

	public static ChestUI instance;

	public Transform itemsParent;

	InventorySlot[] slots;

	InventoryUI inventoryUI;
	ChestInventoryManager chestInventoryManager;

	public GameObject chestUI;
	public GameObject tooltip;

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {

		inventoryUI = InventoryUI.instance;
		chestInventoryManager = ChestInventoryManager.instance;

		chestInventoryManager.onChestItemChangedCallback += UpdateUI;

		slots = itemsParent.GetComponentsInChildren<InventorySlot>();

		DefineSlotIndexes();

	}
	
	// Update is called once per frame
	void Update () {

		if(inventoryUI.itemOnPointer && inventoryUI.currentStackedItemDivision != null){
			DeactivateTooltip(); 
		}

	}

	void DefineSlotIndexes(){

		for(int i = 0; i < slots.Length; i++){
			slots[i].slotIndex = i;
			slots[i].GetComponentInChildren<ItemIcon>().chestItem = true;
			slots[i].chestSlot = true;
		}

	}

	void UpdateUI(){

		for(int i = 0; i < slots.Length; i++){

			if(chestInventoryManager.items[i].itemData != null){
				slots[i].AddItem(chestInventoryManager.items[i]);
			}else{
			   slots[i].ClearSlot();
			}

		}

	}

	public void OpenChestInventory(){
		inventoryUI.OpenInventory();
		chestUI.SetActive(true);
	}

	public void HideInventory(){

		chestInventoryManager.UpdateChestItems();

		chestUI.SetActive(false);
	}

	public void InsertItemOnPointer(InventoryItem item){

		//Get the mouse position
		inventoryUI.InsertItemOnPointer(item);

	}

	public void DivideStackableItem(InventorySlot originalSlot, int amount){
		
		if(!inventoryUI.itemOnPointer){ //Divide the stack for the first time

			InsertItemOnPointer(new InventoryItem(originalSlot.item.itemData, amount));

			chestInventoryManager.DecreaseItemAmount(originalSlot.slotIndex, amount);

		}else{

			if(inventoryUI.currentStackedItemDivision != null){
				if(inventoryUI.currentStackedItemDivision.item.itemData.itemName == originalSlot.item.itemData.itemName){

					chestInventoryManager.DecreaseItemAmount(originalSlot.slotIndex, amount);
					inventoryUI.currentStackedItemDivision.IncreaseAmount(amount);

				}
			}

		}
	}

	public void SwapItemOnPointer(InventorySlot slot){

		InventoryItem slotItem = slot.item;

		chestInventoryManager.AddItemToSlot(inventoryUI.currentStackedItemDivision.item, slot.slotIndex);
		inventoryUI.currentStackedItemDivision.SetItem(slotItem);

	}

	public void DecreaseItemOnPointerAmount(int amount){

		inventoryUI.DecreaseItemOnPointerAmount(amount);
	}

	public void ClearItemOnPointer(){

		Destroy(inventoryUI.currentStackedItemDivision.gameObject);
		inventoryUI.currentStackedItemDivision = null;
		inventoryUI.itemOnPointer = false;

	}

	public InventoryItem GetCurrentStackDivisionItem(){

		return inventoryUI.currentStackedItemDivision.item;
		
	}

	public void ActivateTooltip(InventoryItem item){
		tooltip.GetComponent<InventoryTooltip>().Activate(item);
	}

	public void DeactivateTooltip(){
		tooltip.GetComponent<InventoryTooltip>().Deactivate();
	}

}
