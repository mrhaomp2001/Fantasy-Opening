using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingController : Singleton<FishingController>, IUpdatable
{
    [SerializeField] private bool isFishing;

    [SerializeField] private Transform transformFishingRig;
    [Header("--")]
    [SerializeField] private Transform transformFishingBobber;
    [SerializeField] private LineRenderer lineFishing;
    [SerializeField] private Transform transformFishingIndicator;
    [SerializeField] private SpriteRenderer spriteFishingIndicator;
    [Header("--")]
    [SerializeField] private RectTransform containerFishingUI;
    public bool IsFishing { get => isFishing; set => isFishing = value; }

    public void UpdateViews()
    {

        // Fishing
        transformFishingIndicator.gameObject.SetActive(false);

        containerFishingUI.gameObject.SetActive(false);
        var itemHolding = InventoryController.Instance.GetPlayerData.SelectedHotbar.item;
        if (itemHolding is ItemFishingRod fishingRod)
        {
            transformFishingIndicator.gameObject.SetActive(true);
            containerFishingUI.gameObject.SetActive(true);


        }
    }


    public void StartFishing()
    {
        if (!IsFshingValid())
        {
            return;
        }

        spriteFishingIndicator.gameObject.SetActive(false);

        isFishing = true;

        transformFishingRig.gameObject.SetActive(true);

        UpdateLine();

    }

    public void StopFishing()
    {
        isFishing = false;

        transformFishingRig.gameObject.SetActive(false);

        spriteFishingIndicator.gameObject.SetActive(true);

    }

    public void UpdateLine()
    {
        lineFishing.SetPosition(0, PlayerController.Instance.SpriteItemHolding.transform.position);
        lineFishing.SetPosition(1, transformFishingBobber.position);
    }

    public bool IsFshingValid()
    {
        return BuildingController.Instance.IsCellEmpty();
    }

    public void OnUpdate()
    {
        if (InventoryController.Instance != null)
        {
            if (InventoryController.Instance.GetPlayerData != null && InventoryController.Instance.GetPlayerData.SelectedHotbar != null)
            {
                if (InventoryController.Instance.GetPlayerData.SelectedHotbar.item != null)
                {
                    if (InventoryController.Instance.GetPlayerData.SelectedHotbar.item is ItemFishingRod fishingRod)
                    {
                        Vector3Int cellPosition = BuildingController.Instance.GridBuilding.WorldToCell(PlayerController.Instance.FirepointHitbox.transform.position);

                        transformFishingIndicator.transform.position = BuildingController.Instance.GridBuilding.GetCellCenterWorld(cellPosition);
                    }

                    if (BuildingController.Instance.IsCellEmpty())
                    {
                        spriteFishingIndicator.color = Color.green;
                    }
                    else
                    {
                        spriteFishingIndicator.color = Color.red;
                    }
                }
            }
        }
    }

    private void OnEnable()
    {
        UpdateController.Instance.Updatables.Add(this);
    }

    private void OnDisable()
    {
        UpdateController.Instance.Updatables.Remove(this);
    }
}
