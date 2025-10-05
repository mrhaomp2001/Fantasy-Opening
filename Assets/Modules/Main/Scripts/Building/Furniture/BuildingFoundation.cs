using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingFoundation : BuildingBase, IWorldInteractable, IPoolObject
{

    [SerializeField] private TileBase tilePlayerBuild;

    public void OnObjectSpawnAfter()
    {
        BuildingController.Instance.BuildTile(transform.position, tilePlayerBuild);

        gameObject.SetActive(false);
    }
}
