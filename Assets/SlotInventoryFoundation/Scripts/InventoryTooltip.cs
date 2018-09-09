using UnityEngine;
using TMPro;

public class InventoryTooltip : MonoBehaviour {

	public TextMeshProUGUI description;

	private Vector3 mousePosition;

	void Update(){
		if(gameObject.activeSelf){
			mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)); 
			transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
		}
	}

	public void Activate(InventoryItem item){

		mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)); 
		transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
		gameObject.SetActive(true);
		description.text = item.itemData.description;

	}

	public void Deactivate(){
		gameObject.SetActive(false);

	}
}
