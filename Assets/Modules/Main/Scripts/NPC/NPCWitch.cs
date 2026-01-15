using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWitch : NPC
{
    [Header("NPCWitch: ")]
    [SerializeField] private List<Dialogue> dialogues1;
    [SerializeField] private Dialogue dialogueOption1;

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
                message = LanguageController.Instance.GetString("npc_talk"),
                action = () =>
                {


                }
            },
            new ActionWithMessage
            {
                message = LanguageController.Instance.GetString("npc_player_ascension"),
                action = () =>
                {
                    PopUpAscension.Instance.Show();
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
