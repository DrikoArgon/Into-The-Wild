using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {

	public string itemName = "New Item";
	public Sprite icon = null;

	[TextArea]
	public string description = "Item Description";

	public int cost = 0;
	public int goldValue = 0;
	public int spiritValue = 0;
	public int quality = 0;
	public bool stackable = true;

	public GameObject pickUpPrefab;

	public virtual void Use(){

	}

	public virtual void Consume(){
		
	}
}
