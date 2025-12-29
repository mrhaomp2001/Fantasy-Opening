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

    [Header("Option: ")]
    [SerializeField] private RectTransform buttonOption4;
    [SerializeField] private TextMeshProUGUI textOption4;

    [Header("Option: ")]
    [SerializeField] private RectTransform buttonOption5;
    [SerializeField] private TextMeshProUGUI textOption5;

    [Header("Option: ")]
    [SerializeField] private RectTransform buttonOption6;
    [SerializeField] private TextMeshProUGUI textOption6;

    private ActionWithMessage action1, action2, action3, action4, action5, action6;


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

    public void ShowDialogue(Dialogue dialogue, ActionWithMessage actionOption1, ActionWithMessage actionOption2 = null, ActionWithMessage actionOption3 = null, ActionWithMessage actionOption4 = null, ActionWithMessage actionOption5 = null, ActionWithMessage actionOption6 = null)
    {
        ResetDialogue();
        base.Show();
        textName.SetText(LanguageController.Instance.GetString(dialogue.Name));
        textContent.SetText(LanguageController.Instance.GetString(dialogue.Content));

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
        action4 = actionOption4;
        action5 = actionOption5;
        action6 = actionOption6;

        UpdateViews();

        //
        var enemies = FindObjectsByType<Enemy>(findObjectsInactive: FindObjectsInactive.Include, sortMode: FindObjectsSortMode.None);

        foreach (var item in enemies)
        {
            item.CanMove = false;
        }
        //
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
            case 4:
                action4?.action?.Invoke();
                break;
            case 5:
                action5?.action?.Invoke();
                break;
            case 6:
                action6?.action?.Invoke();
                break;
            default:
                //Debug.LogWarning("Invalid choice made in dialogue options.");
                break;
        }
        Hide();

        //
        var enemies = FindObjectsByType<Enemy>(findObjectsInactive: FindObjectsInactive.Include, sortMode: FindObjectsSortMode.None);

        foreach (var item in enemies)
        {
            item.CanMove = true;
        }
        //
    }

    public void UpdateViews()
    {
        if (action1 != null)
        {
            textOption1.SetText(action1.message);
            buttonOption1.gameObject.SetActive(true);
        }

        if (action2 != null)
        {
            textOption2.SetText(action2.message);
            buttonOption2.gameObject.SetActive(true);
        }

        if (action3 != null)
        {
            textOption3.SetText(action3.message);
            buttonOption3.gameObject.SetActive(true);
        }

        if (action4 != null)
        {
            textOption4.SetText(action4.message);
            buttonOption4.gameObject.SetActive(true);
        }

        if (action5 != null)
        {
            textOption5.SetText(action5.message);
            buttonOption5.gameObject.SetActive(true);
        }

        if (action6 != null)
        {
            textOption6.SetText(action6.message);
            buttonOption6.gameObject.SetActive(true);
        }
    }

    public void ResetDialogue()
    {
        action1 = null;
        action2 = null;
        action3 = null;
        action4 = null;
        action5 = null;
        action6 = null;

        textName.SetText(string.Empty);
        textContent.SetText(string.Empty);

        buttonOption1.gameObject.SetActive(false);
        buttonOption2.gameObject.SetActive(false);
        buttonOption3.gameObject.SetActive(false);
        buttonOption4.gameObject.SetActive(false);
        buttonOption5.gameObject.SetActive(false);
        buttonOption6.gameObject.SetActive(false);
    }
}
