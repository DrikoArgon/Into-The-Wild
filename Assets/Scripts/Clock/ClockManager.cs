using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockManager : MonoBehaviour {

	public static ClockManager instance;

	public Season currentSeason = Season.Spring;

	public int currentYear = 1;

	public int currentDay = 1;
	public DayOfTheWeek currentDayOfTheWeek = DayOfTheWeek.Mon;

	public int currentHour = 6;
	public int currentMinutes = 0;
	public TimeOfTheDay currentTimeOfTheDay = TimeOfTheDay.AM;


	public delegate void OnTimeChanged();
	public OnTimeChanged onTimeChangedCallback;

	public bool timeActivated;
	public bool dangerousTime;

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {

		StartCoroutine(TickClock());
	}

	public void InitializeClock(){

		currentHour = 6;
		currentMinutes = 0;
		currentTimeOfTheDay = TimeOfTheDay.AM;

		currentDay = 1;
		currentDayOfTheWeek = DayOfTheWeek.Mon;

		currentSeason = Season.Spring;
		currentYear = 1;

	}

	public void NextSeason(){

		currentDay = 1;
		currentDayOfTheWeek = DayOfTheWeek.Mon;
		currentSeason++;

		if((int)currentSeason >= 4){

			currentSeason = Season.Spring;
			currentYear++;
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator TickClock(){

		while(timeActivated){
			yield return new WaitForSeconds(10f);

			currentMinutes += 10;

			if(currentMinutes >= 60){
				HandleHour();

				if(currentHour == 12){
					HandleTimeOfTheDay();
				}

				if(currentHour > 12){
					currentHour = 1;
				}

			}

			if(currentHour == 12 && currentTimeOfTheDay == TimeOfTheDay.AM){
				currentHour = 0;
				SetDangerousTime(true);
			}


			if(currentHour == 2 && currentTimeOfTheDay == TimeOfTheDay.AM){
				FinishDay();
			}

			if(onTimeChangedCallback != null){
				onTimeChangedCallback.Invoke();
			}

		}

	}

	void HandleHour(){
		currentHour++;
		currentMinutes = 0;
	}

	void HandleTimeOfTheDay(){


		if(currentTimeOfTheDay == TimeOfTheDay.AM){

			currentTimeOfTheDay = TimeOfTheDay.PM;
		}else{

			currentTimeOfTheDay = TimeOfTheDay.AM;
		}
	}

	void SetDangerousTime(bool isDangerous){
		dangerousTime = isDangerous;
	}

	void HandleDay(){

		currentDay++;

		currentDayOfTheWeek++;

		if((int)currentDayOfTheWeek >= 7){
			currentDayOfTheWeek = DayOfTheWeek.Mon;
		}

		if(currentDay > 28){
			NextSeason();
		}
	}


	public void FinishDay(){
		currentHour = 6;
		currentMinutes = 0;
		currentTimeOfTheDay = TimeOfTheDay.AM; 
		SetDangerousTime(false);

		HandleDay();
	}
}

public enum Season{
	Spring,
	Summer,
	Fall,
	Winter
}

public enum DayOfTheWeek{
	Mon,
	Tue,
	Wed,
	Thu,
	Fri,
	Sat,
	Sun
}

public enum TimeOfTheDay{
	AM,
	PM
}
