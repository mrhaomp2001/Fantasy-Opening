using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRabbit : NPC
{
    [Header("Rabbit NPC: ")]
    [SerializeField] private List<Dialogue> dialogues1;
    [SerializeField] private Dialogue dialogueOption1;
    [SerializeField] private List<Dialogue> dialogues2;
    [Header("Rabbit Events: ")]
    [SerializeField] private List<Recipe> recipes;
    [SerializeField] private List<InventoryController.InventoryItem> itemsCanSell;


    public override void Interact()
    {
        base.Interact();
        PopUpDialogue.Instance.ShowDialogue(dialogues1);
    }

    public void StartOption1()
    {
        PopUpDialogueOption.Instance
            .ShowDialogue(dialogueOption1,
            new ActionWithMessage
            {
                message = LanguageController.Instance.GetString("npc_rabbit_choice_1"),
                action = () =>
                {
                    PopUpDialogue.Instance.ShowDialogue(dialogues2);

                }
            },
            new ActionWithMessage
            {
                message = LanguageController.Instance.GetString("npc_rabbit_choice_2"),
                action = () =>
                {
                    PopUpInventory.Instance.TurnPopUp();

                    PopUpInventory.Instance.TurnCrafting(recipes, isHideInventoryOptions: true);
                }
            },
            new ActionWithMessage
            {
                message = LanguageController.Instance.GetString("npc_rabbit_choice_3"),
                action = () =>
                {
                    PopUpInventory.Instance.TurnPopUp();

                    PopUpInventory.Instance.TurnSelling(itemsCanSell);
                }
            });
    }
}
