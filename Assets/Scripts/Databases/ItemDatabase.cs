using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class ItemDatabase : MonoBehaviour {

	public static ItemDatabase instance;


	public List<Item> database = new List<Item>();
	JsonData itemData;
}
//	void Awake(){
//
//		if(!ItemDatabase.instance){
//
//			instance = this;
//			DontDestroyOnLoad(this);
//		}
//
//	}
//
//	public void CreateDatabase(){
//
//		if(database.Count == 0){
//
//			itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
//			ConstructItemDatabase();
//
//		}
//	}
//
//
//	void ConstructItemDatabase(){
//
//		for(int i = 0; i < itemData.Count; i++){
//			database.Add(new Item(
//				(int)itemData[i]["id"],
//
//				itemData[i]["title"].ToString(),
//
//				itemData[i]["description"].ToString(),
//
//				(int)itemData[i]["cost"],
//
//				(int)itemData[i]["value"],
//				 
//				(bool)itemData[i]["stackable"],
//
//				(int)itemData[i]["quality"],
//
//				itemData[i]["slug"].ToString()
//				)
//			);
//		}
//
//	}
//
//	public Item FetchItemByID(int id){
//
//		Item item = database.Find(x => x.ID == id);
//
//		if(item != null){
//			return item;
//		}else{
//			return null;
//		}
//	}
//}

/*
public class Item{

	public int ID {get; set;}
	public string Title {get; set;}
	public string Description {get; set;}
	public int Cost {get; set;}
	public int Value {get; set;}
	public bool Stackable {get; set;}
	public int Quality {get; set;}
	public string Slug {get; set;}
	public Sprite Sprite {get; set;}

	public Item(int id, string title, string description,int cost, int value, bool stackable, int quality, string slug){

		this.ID = id;
		this.Title = title;
		this.Description = description;
		this.Cost = cost;
		this.Value = value;
		this.Stackable = stackable;
		this.Quality = quality;
		this.Slug = slug;
		this.Sprite = Resources.Load<Sprite>("Items/" + slug);

	}

	public Item(){
		this.ID = -1;
	}

}
*/