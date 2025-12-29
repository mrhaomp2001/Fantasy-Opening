using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpTransition : PopUp
{
    private static PopUpTransition instance;
    [Header("Transition: ")]

    [SerializeField] private Image imageBackground;

    public static PopUpTransition Instance { get => instance; set => instance = value; }

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

    public void StartTransition(Action onTransition)
    {
        base.Show();
        try
        {
            imageBackground.color = new Color(0f, 0f, 0f, 0f);

            LeanTween.cancel(imageBackground.gameObject);
            LeanTween.alpha(imageBackground.rectTransform, 1f, 0.5f)
                .setOnComplete(() =>
                {
                    onTransition?.Invoke();

                    LeanTween.delayedCall(0.5f, () =>
                    {
                        if (imageBackground != null)
                        {
                            LeanTween.alpha(imageBackground.rectTransform, 0f, 1f)
                            .setOnComplete(() =>
                            {
                                base.Hide();
                            });
                        }
                    });
                });
        }
        catch (Exception e)
        {
            Debug.Log($"Exception: {e.Message}");
            throw;
        }

    }
}
