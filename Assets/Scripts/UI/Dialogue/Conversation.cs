using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "New Conversation", menuName = "Dialogue/Conversation")]
public class Conversation : ScriptableObject {

	public List<Sentence> sentences;


}

[System.Serializable]
public struct Sentence{

	public string characterName;

	public Sprite characterImage;

	[TextAreaAttribute]
	public string text;

	public bool appearRight;

	public bool hasChoice;

	public List<Choice> choices;

}

