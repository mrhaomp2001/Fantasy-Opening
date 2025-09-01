using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingWithCrafting : BuildingBase, IWorldInteractable, IPoolObject
{

    [SerializeField] private List<Recipe> recipes;
    public void OnObjectSpawnAfter()
    {

    }

    public void OnWorldInteract()
    {
        PopUpInventory.Instance.TurnPopUp();

        PopUpInventory.Instance.TurnCrafting(recipes, isHideInventoryOptions: true);
    }
}
