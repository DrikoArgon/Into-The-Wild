using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PlayerStatsUI : MonoBehaviour {

	public Image healthBar;
	public Image energyBar;

	public TextMeshProUGUI healthText;
	public TextMeshProUGUI energyText;

	private int lastHealth;
	private int lastEnergy;

	public Text spritualLevelText;

	private Coroutine hpSliderCoroutine;
	private Coroutine evergySliderCoroutine;

	// Use this for initialization
	void Start () {

		PlayerStats.instance.onStatsChangedCallback += UpdateUI;
		UpdateUI();
		lastHealth = PlayerStats.instance.currentHealth;
		lastEnergy = PlayerStats.instance.currentEnergy;
	}
	
	// Update is called once per frame
	void UpdateUI() {

		if(lastHealth != PlayerStats.instance.currentHealth){

			healthText.text = PlayerStats.instance.currentHealth + "/" + PlayerStats.instance.maxHealth;

			if(hpSliderCoroutine != null){
				StopCoroutine(hpSliderCoroutine);
			}

			hpSliderCoroutine = StartCoroutine(UpdateHpSlider());
		}

		if(lastEnergy != PlayerStats.instance.currentEnergy){

			energyText.text = PlayerStats.instance.currentEnergy + "/" + PlayerStats.instance.maxEnergy;

			if(evergySliderCoroutine != null){
				StopCoroutine(evergySliderCoroutine);
			}

			evergySliderCoroutine = StartCoroutine(UpdateEnergySlider());
		}




//		healthBar.fillAmount = (float)PlayerStats.instance.currentHealth / PlayerStats.instance.maxHealth;
//		energyBar.fillAmount = (float)PlayerStats.instance.currentEnergy / PlayerStats.instance.maxEnergy;
	}

	IEnumerator UpdateHpSlider(){

		float lastPercentage = (float)lastHealth / PlayerStats.instance.maxHealth;
		float newPercentage = (float)PlayerStats.instance.currentHealth / PlayerStats.instance.maxHealth;


		if(lastPercentage > newPercentage){

			while(newPercentage < lastPercentage){

				lastPercentage -= Time.deltaTime;

				if(lastPercentage < newPercentage){
					lastPercentage = newPercentage;
				}

				lastHealth = (int)(lastPercentage * PlayerStats.instance.maxHealth); 
				healthBar.fillAmount = lastPercentage;
				yield return null;
			}

		}else{

			while(newPercentage > lastPercentage){

				lastPercentage += Time.deltaTime;

				if(lastPercentage > newPercentage){
					lastPercentage = newPercentage;
				}

				lastHealth = (int)(lastPercentage * PlayerStats.instance.maxHealth); 
				healthBar.fillAmount = lastPercentage;
				yield return null;
			}
		}
	
	}

	IEnumerator UpdateEnergySlider(){

		float lastPercentage = (float)lastEnergy / PlayerStats.instance.maxEnergy;
		float newPercentage = (float)PlayerStats.instance.currentEnergy / PlayerStats.instance.maxEnergy;

		if(lastPercentage > newPercentage){

			while(newPercentage < lastPercentage){

				lastPercentage -= Time.deltaTime;

				if(lastPercentage < newPercentage){
					lastPercentage = newPercentage;
				}

				lastEnergy = (int)(lastPercentage * PlayerStats.instance.maxEnergy);
				energyBar.fillAmount = lastPercentage;
				yield return null;
			}

		}else{

			while(newPercentage > lastPercentage){

				lastPercentage += Time.deltaTime;

				if(lastPercentage > newPercentage){
					lastPercentage = newPercentage;
				}

				lastEnergy = (int)(lastPercentage * PlayerStats.instance.maxEnergy); 
				energyBar.fillAmount = lastPercentage;
				yield return null;
			}
		}

	


	}
}
