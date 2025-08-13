using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBed : BuildingBase, IWorldInteractable, IPoolObject
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    public void OnObjectSpawnAfter()
    {
        spriteRenderer.sortingOrder = (int)-(transform.position.y * 100f);
    }

    public void OnWorldInteract()
    {
        Debug.Log("BuildingBed OnWorldInteract");
    }
}
