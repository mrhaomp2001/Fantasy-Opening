using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCraftingStation : NPC
{

    [Header("Events: ")]
    [SerializeField] private List<Recipe> recipes;

    public override void Interact()
    {
        base.Interact();

        PopUpInventory.Instance.TurnPopUp();

        PopUpInventory.Instance.TurnCrafting(recipes, isHideInventoryOptions: true);
    }
}
