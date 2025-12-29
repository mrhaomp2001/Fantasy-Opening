using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCAnna : NPC
{
    [Header("NPCAnna: ")]
    [SerializeField] private List<Dialogue> dialogues1;
    [SerializeField] private Dialogue dialogueOption1;
    [SerializeField] private List<Dialogue> dialogues2;
    [SerializeField] private Dialogue dialogueOption2;

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
                message = LanguageController.Instance.GetString("npc_anna_choice_1_1"),
                action = () =>
                {


                }
            },
            new ActionWithMessage
            {
                message = LanguageController.Instance.GetString("npc_anna_choice_1_2"),
                action = () =>
                {
                    PopUpInventory.Instance.TurnPopUp();

                    PopUpInventory.Instance.TurnCrafting(recipes, isHideInventoryOptions: true);
                }
            },
            new ActionWithMessage
            {
                message = LanguageController.Instance.GetString("npc_anna_choice_1_3"),
                action = () =>
                {
                    PopUpInventory.Instance.TurnPopUp();

                    PopUpInventory.Instance.TurnSelling(itemsCanSell);
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
                return predicate.Id == 7;
            })
            .FirstOrDefault();

        var item8 = ItemDatabase.Instance.Items
            .Where(predicate =>
            {
                return predicate.Id == 8;
            })
            .FirstOrDefault();

        PopUpDialogueOption.Instance
        .ShowDialogue(dialogueOption2,
        new ActionWithMessage
        {
            message = string.Format(
                LanguageController.Instance.GetString("npc_anna_choice_2_1"),
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
            message = LanguageController.Instance.GetString("npc_anna_choice_2_2"),
            action = () =>
            {

            }
        });
    }
}
