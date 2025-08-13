using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionWithMessage
{
    public string message;
    public Action action;
}
public class PopUpDialogueOption : PopUp
{
    private static PopUpDialogueOption instance;

    [Header("Dialogue Option: ")]

    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private TextMeshProUGUI textContent;
    [SerializeField] private Image imageActor;
    [SerializeField] private Sprite spriteMask64;

    [Header("Option: ")]
    [SerializeField] private RectTransform buttonOption1;
    [SerializeField] private TextMeshProUGUI textOption1;

    [Header("Option: ")]
    [SerializeField] private RectTransform buttonOption2;
    [SerializeField] private TextMeshProUGUI textOption2;

    [Header("Option: ")]
    [SerializeField] private RectTransform buttonOption3;
    [SerializeField] private TextMeshProUGUI textOption3;

    private ActionWithMessage action1, action2, action3;


    public static PopUpDialogueOption Instance { get => instance; set => instance = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowDialogue(Dialogue dialogue, ActionWithMessage actionOption1, ActionWithMessage actionOption2 = null, ActionWithMessage actionOption3 = null)
    {
        ResetDialogue();
        base.Show();
        textName.text = dialogue.Name;
        textContent.text = LanguageController.Instance.GetString(dialogue.Content);

        if (dialogue.Sprite == null)
        {
            imageActor.sprite = spriteMask64;
        }
        else
        {
            imageActor.sprite = dialogue.Sprite;
        }
        action1 = actionOption1;
        action2 = actionOption2;
        action3 = actionOption3;

        UpdateViews();
    }

    public void ChooseAction(int choice)
    {
        switch (choice)
        {
            case 1:
                action1?.action?.Invoke();
                break;
            case 2:
                action2?.action?.Invoke();
                break;
            case 3:
                action3?.action?.Invoke();
                break;
            default:
                Debug.LogWarning("Invalid choice made in dialogue options.");
                break;
        }
        ResetDialogue();
        Hide();
    }

    public void UpdateViews()
    {
        if (action1 != null)
        {
            textOption1.text = action1.message;
            buttonOption1.gameObject.SetActive(true);

        }

        if (action2 != null)
        {
            textOption2.text = action2.message;
            buttonOption2.gameObject.SetActive(true);

        }

        if (action3 != null)
        {
            textOption3.text = action3.message;
            buttonOption3.gameObject.SetActive(true);

        }
    }

    public void ResetDialogue()
    {
        action1 = null;
        action2 = null;
        action3 = null;
        textName.text = string.Empty;
        textContent.text = string.Empty;
        buttonOption1.gameObject.SetActive(false);
        buttonOption2.gameObject.SetActive(false);
        buttonOption3.gameObject.SetActive(false);
    }
}
