using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpRaycastBlocker : PopUp
{

    private static PopUpRaycastBlocker instance;
    [SerializeField] private Image raycastBlocker;

    public static PopUpRaycastBlocker Instance { get => instance; set => instance = value; }
    public Image RaycastBlocker { get => raycastBlocker; set => raycastBlocker = value; }

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

    public override void Show()
    {
        base.Show();
        RaycastBlocker.gameObject.SetActive(true);
        RaycastBlocker.color = new Color(0f, 0f, 0f, 1f);

    }

    public override void Hide()
    {
        FadeOutImage();
    }

    public void FadeOutImage()
    {
        LeanTween.cancel(RaycastBlocker.gameObject);
        LeanTween.value(RaycastBlocker.gameObject, 1f, 0f, 2f)
            .setOnUpdate((float value) =>
            {
                RaycastBlocker.color = new Color(0f, 0f, 0f, value);
            })
            .setOnComplete(() =>
            {
                RaycastBlocker.gameObject.SetActive(false);
                base.Hide();

            });
    }
}
