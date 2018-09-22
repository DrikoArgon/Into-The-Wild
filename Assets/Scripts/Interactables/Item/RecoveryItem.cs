using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/RecoveryItem")]
public class RecoveryItem : Item {

	public int healthRecoveryAmount;
	public int energyRecoveryAmount;


	public override void Use ()
	{
		InventoryUI.instance.HideInventory();
		NotificationManager.instance.ShowUseItemNotification(icon);

	}

	public override void Consume(){

		PlayerStats.instance.Recover(healthRecoveryAmount, energyRecoveryAmount);
	}
}
