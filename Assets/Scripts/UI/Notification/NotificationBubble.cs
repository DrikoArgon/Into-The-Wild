using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class NotificationBubble : MonoBehaviour {

	public Image icon;
	public TextMeshProUGUI messageText;

	public void SetNotificationInfo(Sprite sprite, string message){
		icon.sprite = sprite;
		messageText.text = message;
	}

	void DestroyAfterAnimation(){
		Destroy(gameObject);
	}
}
