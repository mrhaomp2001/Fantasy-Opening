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
            raycastBlocker.gameObject.SetActive(true);

            raycastBlocker.color = new Color(0f, 0f, 0f, 1f);

            PopUpDialogue.Instance.ShowDialogue(dialogues1);

            OnActived();
        }
    }
    public override void OnLoad()
    {
        base.OnLoad();

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
        LeanTween.cancel(raycastBlocker.gameObject);
        LeanTween.value(raycastBlocker.gameObject, 1f, 0f, 2f)
            .setOnUpdate((float value) =>
            {
                raycastBlocker.color = new Color(0f, 0f, 0f, value);
            })
            .setOnComplete(() =>
            {
                raycastBlocker.gameObject.SetActive(false);
            });
    }

}
