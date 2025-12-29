using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCEnchantedPea : NPC
{
    [SerializeField] private Dialogue dialogueEvent3;
    [SerializeField] private List<Dialogue> dialogues_event_3_1;

    public override void Interact()
    {
        base.Interact();

        var progresstion = ProgressionController.Instance.Progressions
        .Where(predicate =>
        {
            return predicate.ProgressionName.Equals("event_3");
        })
        .FirstOrDefault();

        if (progresstion.IsActivated && !progresstion.IsCompleted)
        {
            PopUpDialogueOption.Instance.ShowDialogue(dialogueEvent3,
                new ActionWithMessage
                {
                    message = string.Format(LanguageController.Instance.GetString("event_option_3_1"), $"{ItemDatabase.Instance.Items.Where(predicate => { return predicate.Id == 409; }).FirstOrDefault().ItemName}"),
                    action = () =>
                    {
                        InventoryController.Instance.Consume(409, 1, new Callback
                        {
                            onSuccess = () =>
                            {
                                PopUpDialogue.Instance.ShowDialogue(dialogues_event_3_1);

                                progresstion.OnCompleted();

                            },
                            onFail = (message) =>
                            {

                            },
                            onNext = () =>
                            {

                            }
                        });

                    }
                },
                new ActionWithMessage
                {
                    message = string.Format(LanguageController.Instance.GetString("event_option_3_2"), $"{ItemDatabase.Instance.Items.Where(predicate => { return predicate.Id == 409; }).FirstOrDefault().ItemName}"),
                    action = () =>
                    {

                    }
                });
        }
        else if (progresstion.IsActivated && progresstion.IsCompleted)
        {

        }


    }
}
