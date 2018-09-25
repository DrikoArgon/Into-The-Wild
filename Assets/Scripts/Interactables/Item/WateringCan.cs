using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Tool/WateringCan")]
public class WateringCan : Tool {

	
	public override void Use ()
	{
		if(PlayerTileSelector.instance.currentFarmableTileSelected != null){
			PlayerTileSelector.instance.currentFarmableTileSelected.WaterSoil();
		}

	}
}
