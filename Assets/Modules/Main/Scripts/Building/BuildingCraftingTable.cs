using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCraftingTable : BuildingBase, IWorldInteractable, IPoolObject
{
    public void OnObjectSpawnAfter()
    {

    }

    public void OnWorldInteract()
    {
        Debug.Log("BuildingCraftingTable OnWorldInteract");
    }
}