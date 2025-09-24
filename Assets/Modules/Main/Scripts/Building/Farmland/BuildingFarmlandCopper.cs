using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingFarmlandCopper : BuildingFarmland
{
    public override void OnSowSeedSuccess()
    {
        base.OnSowSeedSuccess();

        if (Random.Range((int)0, 4) == 0)
        {
            CurrentDay++;
            UpdateViews();
        }

    }

    public override void OnHarvestSuccess()
    {
        base.OnHarvestSuccess();
    }
}
