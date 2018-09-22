using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class PlayerStatusBar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public TextMeshProUGUI text;

	// Use this for initialization
	void Start () {

	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		text.gameObject.SetActive(true); 
	}


	public void OnPointerExit (PointerEventData eventData)
	{
		text.gameObject.SetActive(false);
	}
}
