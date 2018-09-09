using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/RecoveryItem")]
public class RecoveryItem : Item {

	public int healthRecoveryAmount;

	public override void Use ()
	{
		base.Use ();
		Debug.Log("Recovered " + healthRecoveryAmount + " health!");
	}
}
