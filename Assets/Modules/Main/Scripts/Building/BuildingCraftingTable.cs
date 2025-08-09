using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCraftingTable : BuildingBase, IWorldInteractable, IPoolObject
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    public void OnObjectSpawnAfter()
    {
        spriteRenderer.sortingOrder = (int)-transform.position.y;
    }

    public void OnWorldInteract()
    {
        Debug.Log("BuildingCraftingTable OnWorldInteract");
    }
}