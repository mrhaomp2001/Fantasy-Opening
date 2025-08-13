using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartDialogue : MonoBehaviour
{
    [SerializeField] private bool isActivated;
    [SerializeField] private List<Dialogue> dialogues1;
    [SerializeField] private Dialogue choose1;
    [SerializeField] private List<Dialogue> dialogues2;

    [SerializeField] private Image image;

    private void Start()
    {
        if (!isActivated)
        {
            PopUpDialogue.Instance.ShowDialogue(dialogues1);
            image.gameObject.SetActive(true);
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
                    PopUpDialogue.Instance.ShowDialogue(dialogues2);
                }
            });
    }

    public void FadeOutImage()
    {
        LeanTween.cancel(image.gameObject);
        LeanTween.value(image.gameObject, 1f, 0f, 2f)
            .setOnUpdate((float value) =>
            {
                image.color = new Color(0f, 0f, 0f, value);
            })
            .setOnComplete(() =>
            {
                image.gameObject.SetActive(false);
            });
    }
}
