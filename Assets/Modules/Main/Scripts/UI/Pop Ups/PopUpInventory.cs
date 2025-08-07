using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpInventory : PopUp
{
    private static PopUpInventory instance;

    [Header("PopUpInventory: ")]
    [SerializeField] private RectTransform contentInventory;
    [SerializeField] private List<HotbarItem> hotbarItem;
    [SerializeField] private List<InventoryGridviewItem> inventoryGridviewItems;
    [SerializeField] private SpriteRenderer transformBuildingIndicator;
    [SerializeField] private SpriteRenderer spriteBuildingReview;


    public static PopUpInventory Instance { get => instance; set => instance = value; }

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsOpening
    {
        get
        {
            return container.gameObject.activeSelf;
        }
    }

    public List<HotbarItem> HotbarItem { get => hotbarItem; set => hotbarItem = value; }
    public SpriteRenderer TransformBuildingIndicator { get => transformBuildingIndicator; set => transformBuildingIndicator = value; }

    public void TurnPopUp()
    {
        if (!container.gameObject.activeSelf)
        {
            UpdateViews();
        }

        base.Turn();
    }

    public void UpdateViews()
    {
        foreach (var item in inventoryGridviewItems)
        {
            item.RefreshItem();
        }

        for (int i = 0; i < InventoryController.Instance.GetPlayerData.Items.Count; i++)
        {
            var item = InventoryController.Instance.GetPlayerData.Items[i];

            var gridviewItem = inventoryGridviewItems[i];

            gridviewItem.UpdateViews(item);
        }

    }

    public void UpdateViewHotbar()
    {
        for (int i = 0; i < InventoryController.Instance.GetPlayerData.Hotbar.Count; i++)
        {
            var item = InventoryController.Instance.GetPlayerData.Hotbar[i];

            hotbarItem[i].UpdateViews(item);
        }

        TransformBuildingIndicator.gameObject.SetActive(false);
        var itemBuilding = InventoryController.Instance.GetPlayerData.SelectedHotbar.item;
        if (itemBuilding != null)
        {
            if (itemBuilding is ItemBuilding building)
            {
                TransformBuildingIndicator.gameObject.SetActive(true);
                spriteBuildingReview.sprite = building.BuildingSprite;
            }
        }

    }

    public void ChooseHotbarSlot(int slot)
    {
        HotbarItem[slot].OnClick();
    }
}
