using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProgressionFrozenIsland_2 : ProgressionBase
{
    [SerializeField] private Transform tilemap, worldContent;
    public override void OnLoad()
    {
        base.OnLoad();
        if (IsCompleted)
        {
            tilemap.gameObject.SetActive(true);
            worldContent.gameObject.SetActive(true);
        }

    }
    public override void OnCompleted()
    {
        base.OnCompleted();

        tilemap.gameObject.SetActive(true);
        worldContent.gameObject.SetActive(true);
    }
}
