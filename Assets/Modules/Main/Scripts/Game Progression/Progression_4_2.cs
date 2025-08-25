using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progression_4_2 : ProgressionBase
{
    [Header("Progression_4_2: ")]
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
    }
}
