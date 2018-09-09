using UnityEngine;

public class ItemPickup : Interactable {

	public Item item;
	public int amount = 1;

	public override void Interact ()
	{
		base.Interact ();

		PickUp();

	}

	void PickUp(){

		bool wasPickedUp = Inventory.instance.AddItem(item, amount);

		if(wasPickedUp){
			Debug.Log("Obtained " + item.itemName + ": " + amount);
			Destroy(gameObject);
		}
	
	}
}
