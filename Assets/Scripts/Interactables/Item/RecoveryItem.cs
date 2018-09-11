using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/RecoveryItem")]
public class RecoveryItem : Item {

	public int healthRecoveryAmount;
	public int energyRecoveryAmount;


	public override bool Use ()
	{
		Debug.Log("Trying to use item");
		NotificationManager.instance.ShowUseItemNotification(icon);
		return true;
	}

	public override void Consume(){

		Debug.Log("Recovered " + healthRecoveryAmount + " health!");
		Debug.Log("Recovered " + energyRecoveryAmount + " health!");
	}
}
