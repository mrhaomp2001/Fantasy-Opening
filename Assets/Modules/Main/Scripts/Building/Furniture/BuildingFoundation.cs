using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingFoundation : BuildingBase, IWorldInteractable, IPoolObject
{

    [SerializeField] private TileBase tilePlayerBuild;

    public void OnObjectSpawnAfter()
    {
        BuildTile();
    }

    public void BuildTile()
    {
        BuildingController.Instance.BuildTile(transform.position, tilePlayerBuild);
    }

    public void RemoveTile()
    {

        BuildingController.Instance.RemoveTile(transform.position);
    }
}
