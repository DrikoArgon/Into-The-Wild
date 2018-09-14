using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour {

	public static NotificationManager instance;

	public GameObject notificationBubblePrefab;

	public UseItemNotification useNotification;

	private Inventory inventory;

	void Awake(){
		instance = this;
	}

	void Start(){
		inventory = Inventory.instance;
	}

	public void CreateNotificationOnPanel(Sprite icon, string message){

		GameObject notification = Instantiate(notificationBubblePrefab, transform);

		notification.GetComponent<NotificationBubble>().SetNotificationInfo(icon, message);

	}

	public void ShowUseItemNotification(Sprite icon){

		GameManager.instance.uiActive = true;

		useNotification.gameObject.SetActive(true);
		useNotification.SetupInfo(icon);
	}

	public void UseItemNotificationResult(bool result){

		GameManager.instance.uiActive = false;
		if(result == true){
			inventory.ConsumeItem();
		}
	}

}
