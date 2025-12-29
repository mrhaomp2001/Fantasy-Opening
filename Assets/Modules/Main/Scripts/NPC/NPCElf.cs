using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCElf : NPC
{
    [Header("NPC Elf: ")]
    [SerializeField] private List<Dialogue> dialogues1;
    [SerializeField] private Dialogue dialogueOption1;

    [Header("Events: ")]
    [SerializeField] private List<Recipe> recipes;
    [SerializeField] private List<InventoryController.InventoryItem> itemsCanSell;

    public override void Interact()
    {
        base.Interact();
        PopUpDialogue.Instance.ShowDialogue(dialogues1);

    }

    public void StartChoice1()
    {
        PopUpDialogueOption.Instance
            .ShowDialogue(dialogueOption1,
            new ActionWithMessage
            {
                message = LanguageController.Instance.GetString("npc_player_buy"),
                action = () =>
                {
                    PopUpInventory.Instance.TurnPopUp();

                    PopUpInventory.Instance.TurnCrafting(recipes, isHideInventoryOptions: true);
                }
            },
            new ActionWithMessage
            {
                message = LanguageController.Instance.GetString("npc_player_sell"),
                action = () =>
                {
                    PopUpInventory.Instance.TurnPopUp();

                    PopUpInventory.Instance.TurnSelling(itemsCanSell);
                }
            },
            new ActionWithMessage
            {
                message = LanguageController.Instance.GetString("npc_player_cancel"),
                action = () =>
                {

                }
            });
    }

}
