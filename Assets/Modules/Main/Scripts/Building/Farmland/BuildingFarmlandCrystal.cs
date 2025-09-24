using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingFarmlandCrystal : BuildingFarmland
{
    public override void OnSowSeedSuccess()
    {
        base.OnSowSeedSuccess();
    }

    public override void OnHarvestSuccess()
    {
        if (Random.Range((int)0, 10) == 0)
        {
            WorldItemController.Instance.SpawnItem(CropCurrent.ProductId, transform.position);
        }
        base.OnHarvestSuccess();
    }
}
