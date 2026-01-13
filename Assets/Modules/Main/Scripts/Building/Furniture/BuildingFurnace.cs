using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingFurnace : BuildingBase, IWorldInteractable, IPoolObject
{

    [SerializeField] private List<Recipe> recipes;
    public void OnObjectSpawnAfter()
    {

    }

    public override void OnWorldInteract()
    {
        PopUpInventory.Instance.TurnPopUp();

        PopUpInventory.Instance.TurnCrafting(recipes, isHideInventoryOptions: true);
    }
}
