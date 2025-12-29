using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionFrozenIsland : ProgressionBase
{
    [Header("ProgressionFrozenIsland: ")]

    [SerializeField] private List<Dialogue> dialogue1;
    [SerializeField] private Dialogue dialogueChoice1;

    public override void OnReady()
    {
        base.OnReady();
    }

    public override void OnActived()
    {
        base.OnActived();
    }

    public override void OnLoad()
    {
        base.OnLoad();

    }

    public override void OnCompleted()
    {
        base.OnCompleted();
    }

    public override void OnSave()
    {
        base.OnSave();

        if (IsActivated && !IsCompleted)
        {
            //raycastBlocker.gameObject.SetActive(true);

            //raycastBlocker.color = new Color(0f, 0f, 0f, 1f);

            PopUpRaycastBlocker.Instance.Show();

            PopUpDialogue.Instance.ShowDialogue(dialogue1);

            var enemies = FindObjectsByType<Enemy>(findObjectsInactive: FindObjectsInactive.Include, sortMode: FindObjectsSortMode.None);

            foreach (var item in enemies)
            {
                item.CanMove = false;
            }
        }
    }

    public void ShowChoice1()
    {
        PopUpDialogueOption.Instance.ShowDialogue(dialogueChoice1,
            new ActionWithMessage
            {
                message = LanguageController.Instance.GetString("progression_frozen_island_choice_1_1"),
                action = () =>
                {
                    var progression = ProgressionController.Instance.Progressions
                     .Where(predicate =>
                     {
                         return predicate.ProgressionName.Equals("progression_frozen_island_1");
                     })
                     .FirstOrDefault();

                    progression.OnActived();
                    progression.OnCompleted();

                    FadeOutImage();
                    OnCompleted();
                }
            },
            new ActionWithMessage
            {
                message = LanguageController.Instance.GetString("progression_frozen_island_choice_1_2"),
                action = () =>
                {
                    var progression = ProgressionController.Instance.Progressions
                     .Where(predicate =>
                     {
                         return predicate.ProgressionName.Equals("progression_frozen_island_2");
                     })
                     .FirstOrDefault();

                    progression.OnActived();
                    progression.OnCompleted();

                    FadeOutImage();
                    OnCompleted();
                }
            }
            );
    }

    public void FadeOutImage()
    {
        PopUpRaycastBlocker.Instance.Hide();

        var enemies = FindObjectsByType<Enemy>(findObjectsInactive: FindObjectsInactive.Include, sortMode: FindObjectsSortMode.None);

        foreach (var item in enemies)
        {
            item.CanMove = true;
        }
    }

}
