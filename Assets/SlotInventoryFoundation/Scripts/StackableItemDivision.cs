using UnityEngine;
using UnityEngine.UI;

public class StackableItemDivision : MonoBehaviour {

	public InventoryItem item;

	public void SetItem(InventoryItem item){
		
		this.item = item;

		UpdateInfo();
	}

	public void IncreaseAmount(int amount){
		item.amount += amount;

		UpdateInfo();
	}

	public void DecreaseAmount(int amount){
		item.amount -= amount;

		UpdateInfo();
	}

	public void UpdateInfo(){

		GetComponent<Image>().sprite = item.itemData.icon; 
		GetComponent<Image>().enabled = true;

		GetComponentInChildren<Text>().text = item.amount.ToString();
		if(item.itemData.stackable){
			GetComponentInChildren<Text>().enabled = true;
		}else{
			GetComponentInChildren<Text>().enabled = false;
		}
	}
}
