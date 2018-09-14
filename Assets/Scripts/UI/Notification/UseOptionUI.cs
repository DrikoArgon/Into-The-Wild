using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UseOptionUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler {

	public int optionNumber;

	public UseItemNotification notification;
	// Use this for initialization
	void Start () {

	}


	public void OnPointerClick (PointerEventData eventData)
	{
		notification.SelectOption();
	}


	public void OnPointerEnter (PointerEventData eventData)
	{
		notification.ChangeSelectedOption(optionNumber);
	}

}