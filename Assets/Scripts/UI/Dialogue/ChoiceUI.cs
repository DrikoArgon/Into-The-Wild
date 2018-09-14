using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChoiceUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler {

	public int optionNumber;

	private DialogueManager dialogueManager;
	// Use this for initialization
	void Start () {
		dialogueManager = DialogueManager.instance;
	}


	public void OnPointerClick (PointerEventData eventData)
	{
		dialogueManager.SelectChoice();
	}


	public void OnPointerEnter (PointerEventData eventData)
	{
		dialogueManager.ChangeChoice(optionNumber);
	}

}
