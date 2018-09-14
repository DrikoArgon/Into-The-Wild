using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable {

	new public string name;

	private Animator animator;
	private Rigidbody2D rigidBody;

	public float speed;  

	public List<Item> favouriteItems;
	public List<Item> likedItems;
	public List<Item> hatedItems;

	private int favouriteItemFondness = 5;
	private int likedItemFondness = 3;
	private int neutralItemFondness = 1;

	private int hateFondnessPenalty = 5;

	public int amountOfGiftsReceived;

	public List<Conversation> favouriteGiftConversations;
	public List<Conversation> likedGiftConversations;
	public List<Conversation> neutralGiftConversations;
	public List<Conversation> hatedGiftConversations;

	public List<Conversation> normalConversations;
	public List<Conversation> eventConversations;


	protected virtual void Initialize(){
		amountOfGiftsReceived = 0;
	}

	// Use this for initialization
	void Start () {
		Initialize();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void Interact ()
	{
		Debug.Log("Interacted with NPC");
		DialogueManager.instance.StartDialog(normalConversations[0], name);
	}

	public void ReceiveGift(Item item){

		amountOfGiftsReceived++;

		foreach (Item i in favouriteItems) {
			if(item.itemName == i.itemName){
				NPCRelationshipManager.instance.IncreaseFondnessLevel(favouriteItemFondness, name);
				return;
			}
		}

		foreach (Item i in likedItems) {
			if(item.itemName == i.itemName){
				NPCRelationshipManager.instance.IncreaseFondnessLevel(likedItemFondness, name);
				return;
			}
		}

		foreach (Item i in hatedItems) {
			if(item.itemName == i.itemName){
				NPCRelationshipManager.instance.DecreaseFondnessLevel(hateFondnessPenalty, name);
				return;
			}
		}

		NPCRelationshipManager.instance.IncreaseFondnessLevel(neutralItemFondness, name);

	}

}


