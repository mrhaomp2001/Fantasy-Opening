using GameUtil;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue
{
    [SerializeField] private string name;
    [SerializeField, TextArea(3, 10)] private string content;
    [SerializeField] private Sprite sprite;
    [SerializeField] private UnityEvent unityEventStart, unityEventEnd;


    public string Name { get => name; set => name = value; }
    public string Content { get => content; set => content = value; }
    public Sprite Sprite { get => sprite; set => sprite = value; }
    public UnityEvent UnityEventStart { get => unityEventStart; set => unityEventStart = value; }
    public UnityEvent UnityEventEnd { get => unityEventEnd; set => unityEventEnd = value; }
}
public class PopUpDialogue : PopUp
{
    private static PopUpDialogue instance;

    private Queue<Dialogue> dialogues = new Queue<Dialogue>();

    [Header("Dialogue: ")]

    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private TextMeshProUGUI textContent;
    [SerializeField] private Image imageActor;
    [SerializeField] private Button buttonNext;
    [SerializeField] private Sprite spriteMask64;

    private bool isTyping;
    private string currentContent;
    private int typingId;

    UnityEvent unityEventLastest;
    public static PopUpDialogue Instance { get => instance; set => instance = value; }

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

    public void ShowDialogue(List<Dialogue> dialoguesInput)
    {

        dialogues.Clear();
        foreach (var item in dialoguesInput)
        {
            dialogues.Enqueue(item);
        }

        base.Show();

        if (dialogues.Peek().Sprite == null)
        {
            imageActor.sprite = spriteMask64;
        }
        else
        {
            imageActor.sprite = dialogues.Peek().Sprite;
        }

        textName.SetText(LanguageController.Instance.GetString(dialogues.Peek().Name));

        TypeWriter(dialogues.Peek().Content);

        dialogues.Peek().UnityEventStart?.Invoke();

        DelayButton();

        //
        Timer.DelayFrameAction(4,
            onComplete: () =>
            {
                var enemies = FindObjectsByType<Enemy>(findObjectsInactive: FindObjectsInactive.Include);

                foreach (var item in enemies)
                {
                    item.CanMove = false;
                }
            });

        //
    }

    public void ShowNextDialog()
    {
        if (isTyping)
        {
            typingId++;
            isTyping = false;
            textContent.SetText(
                LanguageController.Instance.GetString(dialogues.Peek().Content)
            );

            DelayButton();
            return;
        }

        if (dialogues.Count > 0)
        {
            dialogues.Peek().UnityEventEnd?.Invoke();

            dialogues.Dequeue();

            if (dialogues.Count > 0)
            {
                dialogues.Peek().UnityEventStart?.Invoke();

                imageActor.sprite = dialogues.Peek().Sprite == null
                    ? spriteMask64
                    : dialogues.Peek().Sprite;

                textName.SetText(
                    LanguageController.Instance.GetString(dialogues.Peek().Name)
                );

                TypeWriter(dialogues.Peek().Content);

                unityEventLastest = dialogues.Peek().UnityEventEnd;
            }
            else
            {
                textName.SetText("");
                textContent.SetText("");
                imageActor.sprite = spriteMask64;

                Timer.DelayFrameAction(6,
                    onComplete: () =>
                    {
                        var enemies = FindObjectsByType<Enemy>(
                            findObjectsInactive: FindObjectsInactive.Include);

                        foreach (var item in enemies)
                        {
                            item.CanMove = true;
                        }
                    });

                Hide();
            }
        }
    }

    public void DelayButton()
    {
        buttonNext.interactable = false;
        LeanTween.cancel(buttonNext.gameObject);
        LeanTween.delayedCall(0.5f, () =>
        {
            buttonNext.interactable = true;
        });
    }

    private void TypeWriter(string content)
    {
        typingId++;
        int currentTypingId = typingId;

        isTyping = true;
        currentContent = content;
        textContent.SetText("");

        string localizedContent = LanguageController.Instance.GetString(content);

        for (int i = 0; i < localizedContent.Length; i++)
        {
            int index = i;

            Timer.DelayAction(0.02f * i, () =>
            {
                if (currentTypingId != typingId)
                    return;

                textContent.SetText(localizedContent.Substring(0, index + 1));

                if (index == localizedContent.Length - 1)
                {
                    isTyping = false;
                }
            });
        }
    }
}
