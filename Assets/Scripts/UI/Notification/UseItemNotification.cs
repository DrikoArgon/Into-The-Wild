using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class UseItemNotification : MonoBehaviour {
	
	public GameObject cursor;
	public Transform confirmCursorPosition;
	public Transform cancelCursorPosition;

	public Image itemIcon;

	private bool movingCursor;
	// Update is called once per frame
	public bool acceptSelected;
	private NotificationManager notificationManager;

	public float timeBetweenEachCursorMovement = 0.3f;

	void Start(){
		notificationManager = NotificationManager.instance;
	}

	public void SetupInfo(Sprite icon){
		itemIcon.sprite = icon;
	}

	void Update () {
		if(gameObject.activeSelf){
			if(!movingCursor){
				if(Input.GetButtonDown("MoveRight") || Input.GetButtonDown("MoveLeft")){
					StartCoroutine(MoveCursor());
				}
			}

			if(Input.GetMouseButtonDown(0)){
				if(acceptSelected){
					Accept();
				}else{
					Cancel();
				}
			}
		}
	}

	void Accept(){
		notificationManager.UseItemNotificationResult(true);
		Reset();
	}

	void Cancel(){
		notificationManager.UseItemNotificationResult(false);
		Reset();
	}

	void Reset(){
		acceptSelected = false;
		cursor.transform.position = cancelCursorPosition.position;
		itemIcon.sprite = null;
		movingCursor = false;

		gameObject.SetActive(false);
	}

	IEnumerator MoveCursor(){
		movingCursor = true;

		if(acceptSelected){
			cursor.transform.position = cancelCursorPosition.position;
			acceptSelected = false;
		}else{
			cursor.transform.position = confirmCursorPosition.position;
			acceptSelected = true;
		}

		yield return new WaitForSeconds(timeBetweenEachCursorMovement);
		movingCursor = false;
	}
}
