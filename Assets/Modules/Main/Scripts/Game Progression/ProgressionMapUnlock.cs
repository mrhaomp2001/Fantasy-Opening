using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionMapUnlock : ProgressionBase
{
    [SerializeField] private Transform transformTiles;
    [SerializeField] private Transform transformMapContent;
    public override void OnCompleted()
    {
        base.OnCompleted();

        transformTiles.gameObject.SetActive(true);
        transformMapContent.gameObject.SetActive(true);
    }

    public override void OnLoad()
    {
        base.OnLoad();

        if (IsCompleted && IsSaved)
        {
            transformTiles.gameObject.SetActive(true);
            transformMapContent.gameObject.SetActive(true);
        }

        Debug.Log("OnLoad");
    }

    public override void OnSave()
    {
        base.OnSave();

        if (IsCompleted)
        {
            transformTiles.gameObject.SetActive(true);
            transformMapContent.gameObject.SetActive(true);
        }
    }
}
