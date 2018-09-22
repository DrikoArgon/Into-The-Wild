using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

	public static PlayerStats instance;

	public int spiritualLevel = 1;
	public int maxHealth = 100;
	public int maxEnergy = 100;
	public int currentHealth;
	public int currentEnergy;

	private int healthIncrease = 50;
	private int energyIncrease = 50;

	public delegate void OnStatsChanged();
	public OnStatsChanged onStatsChangedCallback;

	void Awake(){
		instance = this;

	}

	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
		currentEnergy = maxEnergy;
	}

	public void IncreaseLevel(){
		
		spiritualLevel += 1;
		maxHealth += healthIncrease;
		maxEnergy += energyIncrease;

		currentEnergy = maxEnergy;
		currentHealth = maxHealth;

		if(onStatsChangedCallback != null){
			onStatsChangedCallback.Invoke();
		}
	}

	public void Recover(int healthAmount, int energyAmount){

		currentEnergy += energyAmount;

		if(currentEnergy > maxEnergy){
			currentEnergy = maxEnergy;
		}

		currentHealth += healthAmount;

		if(currentHealth > maxHealth){
			currentHealth = maxHealth;
		}

		if(onStatsChangedCallback != null){
			onStatsChangedCallback.Invoke();
		}

	}

	public void Reduce(int healthAmount, int energyAmount){

		currentEnergy -= energyAmount;

		if(currentEnergy < 0){
			currentEnergy = 0;
		}

		currentHealth -= healthAmount;

		if(currentHealth < 0){
			currentHealth = 0;
		}

		if(onStatsChangedCallback != null){
			onStatsChangedCallback.Invoke();
		}

	}


}
