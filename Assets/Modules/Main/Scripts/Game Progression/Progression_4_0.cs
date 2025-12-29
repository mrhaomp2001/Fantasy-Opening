using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Progression_4_0 : ProgressionBase
{
    [Header("Progression_4_0: ")]

    [SerializeField] private Image raycastBlocker;
    [SerializeField] private List<Dialogue> dialogues1;
    [SerializeField] private Dialogue dialogueOption1;
    [SerializeField] private List<Dialogue> dialogues1_1;
    [SerializeField] private List<Dialogue> dialogues1_2;

    public override void OnSave()
    {
        base.OnSave();

        if (IsReady && !IsCompleted)
        {
            PopUpRaycastBlocker.Instance.Show();


            PopUpDialogue.Instance.ShowDialogue(dialogues1);

            OnActived();
            var enemies = FindObjectsByType<Enemy>(findObjectsInactive: FindObjectsInactive.Include, sortMode: FindObjectsSortMode.None);

            foreach (var item in enemies)
            {
                item.CanMove = false;
            }

        }
    }
    public override void OnLoad()
    {
        base.OnLoad();

    }

    public override void OnCompleted()
    {
        base.OnCompleted();
        var enemies = FindObjectsByType<Enemy>(findObjectsInactive: FindObjectsInactive.Include, sortMode: FindObjectsSortMode.None);

        foreach (var item in enemies)
        {
            item.CanMove = true;
        }
    }

    public void StartOption1()
    {
        PopUpDialogueOption.Instance
            .ShowDialogue(dialogueOption1,
            new ActionWithMessage
            {
                message = LanguageController.Instance.GetString("event_4_option_1_1"),
                action = () =>
                {
                    PopUpDialogue.Instance.ShowDialogue(dialogues1_1);

                    var progresstion = ProgressionController.Instance.Progressions
                        .Where(predicate =>
                        {
                            return predicate.ProgressionName.Equals("event_4_1");
                        })
                        .FirstOrDefault();

                    progresstion?.OnActived();
                    progresstion?.OnCompleted();
                    OnCompleted();

                }
            },
            new ActionWithMessage
            {
                message = LanguageController.Instance.GetString("event_4_option_1_2"),
                action = () =>
                {
                    PopUpDialogue.Instance.ShowDialogue(dialogues1_2);

                    var progresstion = ProgressionController.Instance.Progressions
                        .Where(predicate =>
                        {
                            return predicate.ProgressionName.Equals("event_4_2");
                        })
                        .FirstOrDefault();

                    progresstion?.OnActived();
                    progresstion?.OnCompleted();
                    OnCompleted();

                }
            }
            );

    }
    public void FadeOutImage()
    {
        PopUpRaycastBlocker.Instance.Hide();

    }

}
