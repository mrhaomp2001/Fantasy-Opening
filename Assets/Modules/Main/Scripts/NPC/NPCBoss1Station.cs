using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBoss1Station : NPC
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
