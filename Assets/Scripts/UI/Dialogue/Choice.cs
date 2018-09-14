using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Choice{

	[TextArea]
	public string text;

	public Conversation answer;
	public Conversation errorConversation;

	public enum Effect{
		None,
		IncreaseFondness,
		DecreaseFondness,
		GiveItem,
		TakeItem
	};

	public int fondnessAmount;

	public List<Item> itemsToGive;
	public List<Item> itemsToTake;

	public Effect effect;
	
	public void HandleChoiceEffect(string npcName){

		if(effect == Effect.None){
			DialogueManager.instance.StartDialog(answer, npcName);
		}else if(effect == Effect.IncreaseFondness){

			Debug.Log("Increase fondness");
			NPCRelationshipManager.instance.IncreaseFondnessLevel(fondnessAmount, DialogueManager.instance.currentNPCTalking);
			DialogueManager.instance.StartDialog(answer, npcName);

		}else if(effect == Effect.DecreaseFondness){

			Debug.Log("Decrease fondness");
			NPCRelationshipManager.instance.DecreaseFondnessLevel(fondnessAmount, DialogueManager.instance.currentNPCTalking);
			DialogueManager.instance.StartDialog(answer, npcName);

		}else if(effect == Effect.GiveItem){
			Debug.Log("Give item");

			bool result = Inventory.instance.CheckIfCanAddItems(itemsToGive);

			if(result == true){

				for(int i = 0; i < itemsToGive.Count; i++){

					Inventory.instance.AddItem(itemsToGive[i], 1);

				}

			}else{
				DialogueManager.instance.StartDialog(errorConversation, npcName);

			}

		}else if(effect == Effect.TakeItem){
			
		}

	}
}
