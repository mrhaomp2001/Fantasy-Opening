using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCraftingTable : BuildingBase, IWorldInteractable, IPoolObject
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Recipe> recipes;
    public void OnObjectSpawnAfter()
    {
        spriteRenderer.sortingOrder = (int)-(transform.position.y * 100f);
    }

    public override void OnWorldInteract()
    {
        PopUpInventory.Instance.TurnPopUp();

        PopUpInventory.Instance.TurnCrafting(recipes, isHideInventoryOptions: true);
    }
}