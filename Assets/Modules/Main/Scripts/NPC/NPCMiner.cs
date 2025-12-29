using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCMiner : NPC
{
    [Header("NPCMiner: ")]

    [Header("dialogues1: ")]
    [SerializeField] private List<Dialogue> dialogues1;
    [SerializeField] private Dialogue dialogueOption1;

    [Header("dialogues2: ")]
    [SerializeField] private List<Dialogue> dialogues2;
    [SerializeField] private Dialogue dialogueOption2;

    [Header("dialogues3: ")]
    [SerializeField] private List<Dialogue> dialogues3;

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
        var progression = ProgressionController.Instance.Progressions.Where(predicate =>
        {
            return predicate.ProgressionName.Equals("progression_frozen_island");
        }).FirstOrDefault();

        ActionWithMessage actionAvailable = null;

        if (!progression.IsActivated)
        {
            actionAvailable = new ActionWithMessage
            {
                message = LanguageController.Instance.GetString("npc_anna_choice_1_4"),
                action = () =>
                {
                    PopUpDialogue.Instance.ShowDialogue(dialogues2);
                }
            };
        }

        PopUpDialogueOption.Instance
            .ShowDialogue(dialogueOption1,
            new ActionWithMessage
            {
                message = LanguageController.Instance.GetString("npc_talk"),
                action = () =>
                {


                }
            },
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
            },
            actionAvailable);
    }

    public void StartChoice2()
    {
        var item1 = ItemDatabase.Instance.Items
            .Where(predicate =>
            {
                return predicate.Id == 1;
            })
            .FirstOrDefault();

        var item2 = ItemDatabase.Instance.Items
            .Where(predicate =>
            {
                return predicate.Id == 2;
            })
            .FirstOrDefault();

        var item5 = ItemDatabase.Instance.Items
            .Where(predicate =>
            {
                return predicate.Id == 5;
            })
            .FirstOrDefault();

        var item6 = ItemDatabase.Instance.Items
            .Where(predicate =>
            {
                return predicate.Id == 6;
            })
            .FirstOrDefault();

        var item7 = ItemDatabase.Instance.Items
            .Where(predicate =>
            {
                return predicate.Id == 11;
            })
            .FirstOrDefault();

        var item8 = ItemDatabase.Instance.Items
            .Where(predicate =>
            {
                return predicate.Id == 14;
            })
            .FirstOrDefault();

        PopUpDialogueOption.Instance
        .ShowDialogue(dialogueOption2,
        new ActionWithMessage
        {
            message = string.Format(
                LanguageController.Instance.GetString("npc_miner_choice_unlock_island"),
                item1.ItemName, 2,
                item2.ItemName, 2,
                item5.ItemName, 2,
                item6.ItemName, 2,
                item7.ItemName, 2,
                item8.ItemName, 2
            ),
            action = () =>
            {
                bool CheckAndConsumeItems()
                {
                    (int id, int count)[] requiredItems =
                    {
                        (item1.Id, 2),
                        (item2.Id, 2),
                        (item5.Id, 2),
                        (item6.Id, 2),
                        (item7.Id, 2),
                        (item8.Id, 2),
                    };


                    // 1. Kiểm tra trước
                    foreach (var req in requiredItems)
                    {
                        if (!InventoryController.Instance.GetPlayerData.IsItemEnough(req.id, req.count))
                        {
                            return false;
                        }
                    }

                    bool success = true;


                    // 2. Consume từng item
                    foreach (var req in requiredItems)
                    {

                        InventoryController.Instance.Consume(req.id, req.count, new Callback
                        {
                            onSuccess = () =>
                            {
                            },
                            onFail = (message) =>
                            {
                                success = false;
                            },
                            onNext = () =>
                            {
                            }
                        });

                        if (!success)
                        {
                            return false;
                        }
                    }

                    return success;
                }

                if (CheckAndConsumeItems())
                {
                    PopUpDialogue.Instance.ShowDialogue(dialogues3);

                    var progresstion = ProgressionController.Instance.Progressions
                        .Where(predicate =>
                        {
                            return predicate.ProgressionName.Equals("progression_frozen_island");
                        })
                        .FirstOrDefault();

                    progresstion.OnReady();
                    progresstion.OnActived();

                }
                else
                {
                }
            }

        },
        new ActionWithMessage
        {
            message = LanguageController.Instance.GetString("npc_miner_cancel_unlock_island"),
            action = () =>
            {

            }
        });
    }
}
