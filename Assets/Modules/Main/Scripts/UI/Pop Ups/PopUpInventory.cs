using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class PopUpInventory : PopUp
{
    private static PopUpInventory instance;

    [Header("PopUpInventory: ")]
    [SerializeField] private RectTransform contentInventory;
    [SerializeField] private List<HotbarItem> hotbarItem;
    [SerializeField] private List<InventoryGridviewItem> inventoryGridviewItems;
    [SerializeField] private SpriteRenderer transformBuildingIndicator;
    [SerializeField] private SpriteRenderer spriteBuildingReview;

    [Header("Chest: ")]
    [SerializeField] private bool isOpeningChest;
    [SerializeField] private RectTransform containerChest;
    [SerializeField] private BuildingChest currentChest;
    [SerializeField] private List<InventoryGridviewItem> chestGridviewItems;


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
    public bool IsOpeningChest { get => isOpeningChest; set => isOpeningChest = value; }
    public BuildingChest CurrentChest { get => currentChest; set => currentChest = value; }

    public void TurnPopUp(BuildingChest chest = null)
    {
        isOpeningChest = false;
        currentChest = null;
        containerChest.gameObject.SetActive(false);

        if (chest != null)
        {
            isOpeningChest = true;
            currentChest = chest;
            containerChest.gameObject.SetActive(true);

            UpdateViewChest();
        }

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

        var sortedItems = InventoryController.Instance.GetPlayerData.Items
            .OrderByDescending(i => i.item != null)
            .ToList();

        for (int i = 0; i < sortedItems.Count; i++)
        {
            var item = sortedItems[i];

            var gridviewItem = inventoryGridviewItems[i];

            gridviewItem.UpdateViews(item);
        }

        if (currentChest != null)
        {
            UpdateViewChest();

        }
    }

    public void UpdateViewChest()
    {
        foreach (var item in chestGridviewItems)
        {
            item.RefreshItem();
        }
        var sortedItems = currentChest.Items
            .OrderBy(i => i.item == null)
            .ToList();

        for (int i = 0; i < sortedItems.Count; i++)
        {
            Debug.Log($"{i}");

            var item = sortedItems[i];

            var gridviewItem = chestGridviewItems[i];

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
