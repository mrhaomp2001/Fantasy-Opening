using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionFrozenIsland : ProgressionBase
{
    [Header("ProgressionFrozenIsland: ")]

    [SerializeField] private Image raycastBlocker;
    [SerializeField] private Transform tiles3, mapContent3;
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

        tiles3.gameObject.SetActive(false);
        mapContent3.gameObject.SetActive(false);

        if (IsCompleted)
        {
            tiles3.gameObject.SetActive(true);
            mapContent3.gameObject.SetActive(true);
        }
    }

    public override void OnCompleted()
    {
        base.OnCompleted();
    }

    public override void OnSave()
    {
        base.OnSave();

        if (IsCompleted)
        {
            tiles3.gameObject.SetActive(true);
            mapContent3.gameObject.SetActive(true);
        }
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
