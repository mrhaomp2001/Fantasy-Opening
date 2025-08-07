using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBed : BuildingBase, IWorldInteractable, IPoolObject
{
    public void OnObjectSpawnAfter()
    {

    }

    public void OnWorldInteract()
    {
        Debug.Log("BuildingBed OnWorldInteract");
    }
}
