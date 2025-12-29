using GameUtil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionStartDialogue : ProgressionBase
{
    [Header("Start Dialogue: ")]
    [SerializeField] private Transform npcRabbit;
    [SerializeField] private List<Dialogue> dialogues1;
    [SerializeField] private Dialogue choose1;
    [SerializeField] private List<Dialogue> dialogues2;



    public override void OnActived()
    {
        if (!(IsSaved || IsCompleted) && IsActivated)
        {
            var enemies = FindObjectsByType<Enemy>(findObjectsInactive: FindObjectsInactive.Include, sortMode: FindObjectsSortMode.None);

            foreach (var item in enemies)
            {
                item.CanMove = false;
            }

            PopUpDialogue.Instance.ShowDialogue(dialogues1);

            PopUpRaycastBlocker.Instance.Show();
        }

        base.OnActived();
    }
    public override void OnReady()
    {
        base.OnReady();
    }
    public override void OnLoad()
    {
        base.OnLoad();

        OnActived();
    }

    public override void OnCompleted()
    {
        if (!IsSaved && IsActivated)
        {

        }

        base.OnCompleted();
    }

    public override void OnSave()
    {
        base.OnSave();

        if (IsSaved)
        {
            npcRabbit.gameObject.SetActive(true);
        }
    }

    public void ChooseOption1()
    {
        PopUpDialogueOption.Instance.ShowDialogue(choose1,
            new ActionWithMessage()
            {
                message = LanguageController.Instance.GetString("event_1_start_dialogue_choose_1"),
                action = () =>
                {
                    OnCompleted();
                    PopUpDialogue.Instance.ShowDialogue(dialogues2);
                }
            });
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
