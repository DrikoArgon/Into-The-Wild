using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClockUI : MonoBehaviour {

	public TextMeshProUGUI dayText;
	public TextMeshProUGUI dayOfTheWeekText;

	public TextMeshProUGUI seasonText;
	public TextMeshProUGUI yearText;

	public TextMeshProUGUI hourText;
	public TextMeshProUGUI clockIndicationText;
	public TextMeshProUGUI minutesText;
	public TextMeshProUGUI timeOfTheDayText;

	private ClockManager clockManager;
	// Use this for initialization
	void Start () {
		clockManager = ClockManager.instance;

		clockManager.onTimeChangedCallback += UpdateUI;

		UpdateUI();
	}
	
	// Update is called once per frame
	void UpdateUI () {

		//Season and year
		seasonText.text = clockManager.currentSeason.ToString();
		yearText.text = clockManager.currentYear.ToString();

		//Day and Weekday
		dayText.text = clockManager.currentDay.ToString("00");
		dayOfTheWeekText.text = clockManager.currentDayOfTheWeek.ToString();

		//Hour, minutes and time of the day
		hourText.text = clockManager.currentHour.ToString("00");
		minutesText.text = clockManager.currentMinutes.ToString("00");
		timeOfTheDayText.text = clockManager.currentTimeOfTheDay.ToString();

		if(clockManager.dangerousTime){
			hourText.color = new Color(163,0,0,1.0f);
			minutesText.color = new Color(163,0,0,1.0f);
			clockIndicationText.color = new Color(163,0,0,1.0f);
			timeOfTheDayText.color = new Color(163,0,0,1.0f);
		}else{
			hourText.color = new Color(0,0,0,1.0f);
			minutesText.color = new Color(0,0,0,1.0f);
			clockIndicationText.color = new Color(0,0,0,1.0f);
			timeOfTheDayText.color = new Color(0,0,0,1.0f);
		}

	}

}
