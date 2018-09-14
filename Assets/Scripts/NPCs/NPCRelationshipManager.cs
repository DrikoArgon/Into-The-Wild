using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRelationshipManager : MonoBehaviour {

	public static NPCRelationshipManager instance;

	public List<NPCFondnessInfo> npcs;


	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	public int GetFondnessLevel(string name){

		foreach(NPCFondnessInfo npc in npcs){
			if(npc.npcName == name){
				return npc.fondness;
			}
		}

		return -1;
	}

	public void IncreaseFondnessLevel(int amount, string name){

		for(int i = 0; i < npcs.Count; i++){
			if(npcs[i].npcName == name){

				NPCFondnessInfo npcInfo = npcs[i];
				npcInfo.fondness += amount;

				if(npcInfo.fondness > 100){
					npcInfo.fondness = 100;
				}

				npcs[i] = npcInfo;
			}
		}


	}

	public void DecreaseFondnessLevel(int amount, string name){

		for(int i = 0; i < npcs.Count; i++){
			if(npcs[i].npcName == name){
				NPCFondnessInfo npcInfo = npcs[i];
				npcInfo.fondness -= amount;

				if(npcInfo.fondness < 0){
					npcInfo.fondness = 0;
				}

				npcs[i] = npcInfo;
			}
		}
	}

}

[System.Serializable]
public struct NPCFondnessInfo{
	public string npcName;
	public int fondness;
}
