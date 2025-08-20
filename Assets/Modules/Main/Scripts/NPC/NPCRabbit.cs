using GameUtil;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [Header("Event 2: ")]
    [SerializeField] private List<Dialogue> dialogues_event_2_1;
    [SerializeField] private Dialogue dialogue_choice_2_1;
    [SerializeField] private List<Dialogue> dialogues_event_2_2;
    [SerializeField] private Dialogue dialogue_choice_2_2;
    [SerializeField] private List<Dialogue> dialogues_event_2_3;

    [SerializeField] private List<Dialogue> dialogues_event_2_4;


    private ActionWithMessage optionEvent2;

    public override void Interact()
    {
        base.Interact();

        optionEvent2 = null;

        var progresstion = ProgressionController.Instance.Progressions
            .Where(predicate =>
            {
                return predicate.ProgressionName.Equals("event_2");
            })
            .FirstOrDefault();

        if (!progresstion.IsActivated)
        {
            optionEvent2 = new ActionWithMessage
            {
                message = LanguageController.Instance.GetString("npc_rabbit_choice_4_1"),
                action = () =>
                {
                    StartEvent2();
                }
            };
        }
        else if (progresstion.IsActivated && !progresstion.IsCompleted)
        {
            optionEvent2 = new ActionWithMessage
            {
                message = string.Format(LanguageController.Instance.GetString("npc_rabbit_choice_event_2_2"), $"{ItemDatabase.Instance.Items.Where(predicate => { return predicate.Id == 410; }).FirstOrDefault().ItemName}"),
                action = () =>
                {
                    InventoryController.Instance.Consume(410, 1, new Callback
                    {
                        onSuccess = () =>
                        {
                            StartClaimRewardEvent2();

                        },
                        onFail = (message) =>
                        {

                        },
                        onNext = () =>
                        {
                            
                        }
                    });
                }
            };
        }

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
            },
            optionEvent2
            );
    }

    public void StartEvent2()
    {
        PopUpDialogue.Instance.ShowDialogue(dialogues_event_2_1);
    }

    public void Event2Step1()
    {
        PopUpDialogueOption.Instance
            .ShowDialogue(dialogue_choice_2_1,
            new ActionWithMessage
            {
                message = LanguageController.Instance.GetString("npc_rabbit_choice_event_2_1"),
                action = () =>
                {
                    PopUpDialogue.Instance.ShowDialogue(dialogues_event_2_2);
                }
            });
    }

    public void Event2Step2()
    {
        PopUpDialogueOption.Instance
            .ShowDialogue(dialogue_choice_2_2,
            new ActionWithMessage
            {
                message = string.Format(LanguageController.Instance.GetString("npc_rabbit_choice_event_2_2"), $"{ItemDatabase.Instance.Items.Where(predicate => { return predicate.Id == 410; }).FirstOrDefault().ItemName}"),
                action = () =>
                {
                    var progresstion = ProgressionController.Instance.Progressions
                    .Where(predicate =>
                    {
                        return predicate.ProgressionName.Equals("event_2");
                    })
                    .FirstOrDefault();

                    progresstion.IsActivated = true;

                    PopUpDialogue.Instance.ShowDialogue(dialogues_event_2_3);

                }
            }
        );
    }
    public void SpawnCarrot()
    {
        WorldItemController.Instance.SpawnItem(5, PlayerController.Instance.RbPlayer.position, 5);
    }
    public void StartClaimRewardEvent2()
    {
        PopUpDialogue.Instance.ShowDialogue(dialogues_event_2_4);

        var progresstion = ProgressionController.Instance.Progressions
            .Where(predicate =>
            {
                return predicate.ProgressionName.Equals("event_2");
            })
            .FirstOrDefault();

        progresstion.OnCompleted();

        WorldItemController.Instance.SpawnItem(409, PlayerController.Instance.RbPlayer.position, 1);

    }
}
