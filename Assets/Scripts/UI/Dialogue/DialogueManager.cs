using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour {

	public static DialogueManager instance;

	//Dialogue Panel Variables
	public GameObject dialogue;

	public GameObject rightDialogElements;
	public GameObject leftDialogElements;

	public Image rightPortrait;
	public Image leftPortrait;

	public TextMeshProUGUI rightName;
	public TextMeshProUGUI leftName;  

	public TextMeshProUGUI rightMessage;
	public TextMeshProUGUI leftMessage; 

	public float dialogDelay = 0.02f;

	//Choices Panel Variables
	public GameObject choicesPanel;

	public GameObject cursor;
	public TextMeshProUGUI choiceMessage;

	public TextMeshProUGUI option1;
	public Image option1Panel;
	public Transform option1CursorPosition;

	public TextMeshProUGUI option2;
	public Image option2Panel;
	public Transform option2CursorPosition;

	public TextMeshProUGUI option3;
	public Image option3Panel;
	public Transform option3CursorPosition;

	private enum ChoiceSelected{Option1, Option2, Option3};

	private ChoiceSelected currentChoice;

	//States
	private bool isTyping;
	private bool cancelTyping;
	private bool dialogueActive;
	private bool choicesActive;

	//Current info displayed
	public Conversation currentConversation;
	public string currentNPCTalking;
	private int currentConversationSentenceIndex; 

	private bool movingCursor;
	public float timeBetweenEachCursorMovement = 0.3f;

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	void Update()
    {
		if(!dialogueActive && !choicesActive){ //If nothing is active, ignore update function
    		return;
    	}

		if(Input.GetMouseButtonDown(0) && dialogueActive) //If dialog is active, clicking the left mouse button calls next line;
        {
            NextDialogLine();
        }

		if(choicesActive) 
        {
        	if(GameManager.instance.gamepadMode){
				if(!movingCursor){
					
					if(currentConversation.sentences[currentConversationSentenceIndex].choices.Count > 2){

						if(Input.GetButtonDown("MoveRight") || Input.GetButtonDown("MoveDown")){
							StartCoroutine(MoveCursorThreeChoicesPanelDown());
						}else if(Input.GetButtonDown("MoveLeft") || Input.GetButtonDown("MoveUp")){
							StartCoroutine(MoveCursorThreeChoicesPanelUp());
						}

					}else{

						if(Input.GetButtonDown("MoveRight") || Input.GetButtonDown("MoveLeft") || Input.GetButtonDown("MoveDown") || Input.GetButtonDown("MoveUp")){
							StartCoroutine(MoveCursorTwoChoicesPanel());
						}
					}

				}

				if(Input.GetMouseButtonDown(0)){
					SelectChoice();
				}
			}
        }

    }

	public void StartDialog(Conversation conversation, string npcName)
    {

		choicesActive = false;
		choicesPanel.SetActive(false);
		dialogue.SetActive(true);
    	currentConversation = conversation;
		currentNPCTalking = npcName;
    	currentConversationSentenceIndex = 0;

		//Disable character controll and reset variables

		GameManager.instance.uiActive = true;

		Sentence firstSentence = currentConversation.sentences[0];

		SetupDialogueBoxElements(firstSentence);
	
		//Start Dialog
		StartCoroutine (WriteText (firstSentence.text, firstSentence.appearRight));

		dialogueActive = true;
		
    }

    void SetupDialogueBoxElements(Sentence sentence){

    	if(sentence.appearRight){

    		leftDialogElements.SetActive(false);
    		rightDialogElements.SetActive(true);

			rightName.text = sentence.characterName;
			
    		rightPortrait.sprite = sentence.characterImage;

    	}else{

			leftDialogElements.SetActive(true);
    		rightDialogElements.SetActive(false);

 
			leftName.text = sentence.characterName;

			leftPortrait.sprite = sentence.characterImage;
    	}


    }

	public void FinishDialogue()
    {
		dialogueActive = false;
		GameManager.instance.uiActive = false;

		dialogue.SetActive(false);

    }

	public void NextDialogLine(){

		if(!isTyping){

			if(currentConversation.sentences[currentConversationSentenceIndex].choices.Count > 0){
				SetupChoicesPanel(currentConversation.sentences[currentConversationSentenceIndex]);
				return;
			}


			if(currentConversationSentenceIndex < currentConversation.sentences.Count - 1){
				currentConversationSentenceIndex++;

				Sentence nextSentence = currentConversation.sentences[currentConversationSentenceIndex];

				SetupDialogueBoxElements(nextSentence);

				StartCoroutine(WriteText(nextSentence.text, nextSentence.appearRight));

			}else{
				FinishDialogue();
			}
		}else{
			cancelTyping = true;
		}
	}

	void SetupChoicesPanel(Sentence sentence){

		choicesActive = true;
		dialogueActive = false;
		rightDialogElements.SetActive(false);
		leftDialogElements.SetActive(false);
		choicesPanel.SetActive(true);
		choiceMessage.text = sentence.text;

		option1.text = sentence.choices[0].text;
		option2.text = sentence.choices[1].text;

 		if(sentence.choices.Count > 2){
			option3.enabled = true;
			option3.text = sentence.choices[2].text;
		}else{
			option3.enabled = false;
		}

		cursor.transform.position = option1CursorPosition.position;
		option1Panel.color = new Color(option1Panel.color.r, option1Panel.color.g, option1Panel.color.b, 40f/255);
		option2Panel.color = new Color(option2Panel.color.r, option2Panel.color.g, option2Panel.color.b, 0);
		option3Panel.color = new Color(option3Panel.color.r, option3Panel.color.g, option3Panel.color.b, 0);
		currentChoice = ChoiceSelected.Option1;

	}

	public void SelectChoice(){

		Sentence currentSentence = currentConversation.sentences[currentConversationSentenceIndex];

		if(currentChoice == ChoiceSelected.Option1){

			currentSentence.choices[0].HandleChoiceEffect(currentNPCTalking);

		}else if(currentChoice == ChoiceSelected.Option2){

			currentSentence.choices[1].HandleChoiceEffect(currentNPCTalking);

		}else if(currentChoice == ChoiceSelected.Option3){

			currentSentence.choices[2].HandleChoiceEffect(currentNPCTalking);

		}
	}

	public void ChangeChoice(int choiceIndex){

		if(choiceIndex == 1){
			cursor.transform.position = option1CursorPosition.position;
			currentChoice = ChoiceSelected.Option1;
			option1Panel.color = new Color(option1Panel.color.r, option1Panel.color.g, option1Panel.color.b, 40f/255);
			option2Panel.color = new Color(option2Panel.color.r, option2Panel.color.g, option2Panel.color.b, 0);
			option3Panel.color = new Color(option3Panel.color.r, option3Panel.color.g, option3Panel.color.b, 0);
		}else if(choiceIndex == 2){
			cursor.transform.position = option2CursorPosition.position;
			currentChoice = ChoiceSelected.Option2;
			option1Panel.color = new Color(option1Panel.color.r, option1Panel.color.g, option1Panel.color.b, 0);
			option2Panel.color = new Color(option2Panel.color.r, option2Panel.color.g, option2Panel.color.b, 40f/255);
			option3Panel.color = new Color(option3Panel.color.r, option3Panel.color.g, option3Panel.color.b, 0);
		}else if(choiceIndex == 3){
			cursor.transform.position = option3CursorPosition.position;
			currentChoice = ChoiceSelected.Option3;
			option1Panel.color = new Color(option1Panel.color.r, option1Panel.color.g, option1Panel.color.b, 0);
			option2Panel.color = new Color(option2Panel.color.r, option2Panel.color.g, option2Panel.color.b, 0);
			option3Panel.color = new Color(option3Panel.color.r, option3Panel.color.g, option3Panel.color.b, 40f/255);
		}

	}

	IEnumerator MoveCursorTwoChoicesPanel(){
		movingCursor = true;


		if(currentChoice == ChoiceSelected.Option1){
			cursor.transform.position = option2CursorPosition.position;
			currentChoice = ChoiceSelected.Option2;
		}else{
			cursor.transform.position = option1CursorPosition.position;
			currentChoice = ChoiceSelected.Option1;
		}

		yield return new WaitForSeconds(timeBetweenEachCursorMovement);
		movingCursor = false;
	}

	IEnumerator MoveCursorThreeChoicesPanelUp(){
		movingCursor = true;


		if(currentChoice == ChoiceSelected.Option1){
			cursor.transform.position = option3CursorPosition.position;
			currentChoice = ChoiceSelected.Option3;
		}else if(currentChoice == ChoiceSelected.Option2){
			cursor.transform.position = option1CursorPosition.position;
			currentChoice = ChoiceSelected.Option1;
		}else{
			cursor.transform.position = option2CursorPosition.position;
			currentChoice = ChoiceSelected.Option2;
		}

		yield return new WaitForSeconds(timeBetweenEachCursorMovement);
		movingCursor = false;
	}

	IEnumerator MoveCursorThreeChoicesPanelDown(){
		movingCursor = true;

		if(currentChoice == ChoiceSelected.Option1){
			cursor.transform.position = option2CursorPosition.position;
			currentChoice = ChoiceSelected.Option2;
		}else if(currentChoice == ChoiceSelected.Option2){
			cursor.transform.position = option3CursorPosition.position;
			currentChoice = ChoiceSelected.Option3;
		}else{
			cursor.transform.position = option1CursorPosition.position;
			currentChoice = ChoiceSelected.Option1;
		}

		yield return new WaitForSeconds(timeBetweenEachCursorMovement);
		movingCursor = false;
	}


	IEnumerator WriteText(string currentLine, bool showRight){

		int totalVisibleCharacters = 0;
		int counter = 0;

		rightMessage.text = "";
		leftMessage.text = "";

		if(showRight){
			rightMessage.text = currentLine;
		}else{
			leftMessage.text = currentLine;
		}

		totalVisibleCharacters = currentLine.Length;

		isTyping = true;

		int visibleCount = counter % (totalVisibleCharacters + 1);

		while(isTyping && !cancelTyping && (visibleCount < totalVisibleCharacters)){


			visibleCount = counter % (totalVisibleCharacters + 1);

			if(showRight){

				rightMessage.maxVisibleCharacters = visibleCount;

			}else{

				leftMessage.maxVisibleCharacters = visibleCount;
			}

			counter += 1;
			
			yield return new WaitForSeconds(dialogDelay);
		}


		if(showRight){
			rightMessage.maxVisibleCharacters = totalVisibleCharacters;
		}else{
			leftMessage.maxVisibleCharacters = totalVisibleCharacters;
		}

		isTyping = false;
		cancelTyping = false;

	}
}
