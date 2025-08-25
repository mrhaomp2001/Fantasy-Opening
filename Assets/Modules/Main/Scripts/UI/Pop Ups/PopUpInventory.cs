using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static InventoryController;
using static UnityEditor.Progress;

public class PopUpInventory : PopUp
{
    private static PopUpInventory instance;

    [Header("Inventory: ")]
    [SerializeField] private RectTransform contentInventory;
    [SerializeField] private List<HotbarItem> hotbarItem;
    [SerializeField] private List<InventoryGridviewItem> inventoryGridviewItems;
    [SerializeField] private SpriteRenderer transformBuildingIndicator;
    [SerializeField] private SpriteRenderer spriteBuildingReview;

    [Header("Chest: ")]
    [SerializeField] private bool isOpeningChest;
    [SerializeField] private RectTransform containerChest;
    private BuildingChest currentChest;
    [SerializeField] private List<InventoryGridviewItem> chestGridviewItems;

    [Header("Inventory Opttion: ")]
    [SerializeField] private RectTransform containerInventoryOption;

    [Header("Crafting: ")]
    [SerializeField] private RectTransform containerCrafting;
    [SerializeField] private RectTransform containerCraftingTooltip;
    [SerializeField] private List<CraftingGridviewItem> craftingGridviewItems;

    [Header("Selling: ")]
    [SerializeField] private bool isSelling;
    [SerializeField] private RectTransform containerSelling;
    [SerializeField] private List<InventoryGridviewItem> sellingGridviewItems;

    [Header("Equipment: ")]
    [SerializeField] private RectTransform containerEquipment;
    [SerializeField] private List<InventoryGridviewItem> equipmentGridviewItems;
    [Header("Stats: ")]
    [SerializeField] private RectTransform containerPlayerStats;

    [Header("Buffs: ")]
    [SerializeField] private RectTransform containerBuff;
    [SerializeField] private List<InventoryBuffGridviewItem> buffGridviewItems;

    public static PopUpInventory Instance { get => instance; set => instance = value; }

    private void Awake()
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
    public bool IsSelling { get => isSelling; set => isSelling = value; }
    public List<InventoryGridviewItem> SellingGridviewItems { get => sellingGridviewItems; set => sellingGridviewItems = value; }

    public void TurnPopUp(BuildingChest chest = null)
    {
        isOpeningChest = false;
        isSelling = false;
        currentChest = null;

        ResetSubPopUps();


        containerInventoryOption.gameObject.SetActive(true);

        containerEquipment.gameObject.SetActive(true);

        PopUpInventoryCraftingTooltip.Instance.Hide();
        PopUpInventoryTooltip.Instance.Hide();
        PopUpBuffTooltip.Instance.Hide();

        if (chest != null)
        {
            isOpeningChest = true;
            currentChest = chest;
            containerChest.gameObject.SetActive(true);
            containerInventoryOption.gameObject.SetActive(false);
            containerEquipment.gameObject.SetActive(false);

            UpdateViewChest();
        }

        if (!container.gameObject.activeSelf)
        {
            UpdateViews();
        }


        base.Turn();
    }

    public void UpdateViewEquipment()
    {
        equipmentGridviewItems[0].UpdateViews(InventoryController.Instance.GetPlayerData.ArmorHead);
        equipmentGridviewItems[1].UpdateViews(InventoryController.Instance.GetPlayerData.ArmorBody);
        equipmentGridviewItems[2].UpdateViews(InventoryController.Instance.GetPlayerData.ArmorLeg);
        equipmentGridviewItems[3].UpdateViews(InventoryController.Instance.GetPlayerData.ArmorFoot);
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

        PopUpInventoryTooltip.Instance.Hide();


        UpdateViewEquipment();
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
            //Debug.Log($"{i}");

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

    public void ResetSubPopUps()
    {
        containerEquipment.gameObject.SetActive(false);
        containerCrafting.gameObject.SetActive(false);
        containerSelling.gameObject.SetActive(false);
        containerPlayerStats.gameObject.SetActive(false);
        containerBuff.gameObject.SetActive(false);
        containerChest.gameObject.SetActive(false);

        containerInventoryOption.gameObject.SetActive(true);
    }

    public void TurnSelling(List<InventoryItem> itemsCanSell)
    {
        isSelling = true;

        ResetSubPopUps();

        containerInventoryOption.gameObject.SetActive(false);

        containerSelling.gameObject.SetActive(true);


        foreach (var item in sellingGridviewItems)
        {
            item.RefreshItem();
            item.gameObject.SetActive(false);
        }

        for (int i = 0; i < itemsCanSell.Count; i++)
        {
            var itemCanSell = itemsCanSell[i];

            sellingGridviewItems[i].UpdateViews(itemCanSell);
            sellingGridviewItems[i].gameObject.SetActive(true);
        }
    }

    public void TurnCrafting(List<Recipe> valueRecipes, bool isHideInventoryOptions = false)
    {

        if (!containerCrafting.gameObject.activeSelf)
        {
            ResetSubPopUps();

            containerCrafting.gameObject.SetActive(true);

            foreach (var item in craftingGridviewItems)
            {
                if (item.gameObject.activeSelf)
                {
                    item.ResetItem();
                }
            }

            for (int i = 0; i < valueRecipes.Count; i++)
            {
                var gridviewItem = craftingGridviewItems[i];

                gridviewItem.UpdateViews(valueRecipes[i]);
            }

            if (isHideInventoryOptions)
            {
                containerInventoryOption.gameObject.SetActive(false);
            }
        }
        else
        {
            ResetSubPopUps();

            containerEquipment.gameObject.SetActive(true);

        }

    }

    public void TurnStats()
    {
        if (!containerPlayerStats.gameObject.activeSelf)
        {
            ResetSubPopUps();
            containerPlayerStats.gameObject.SetActive(true);

        }
        else
        {
            ResetSubPopUps();
            containerEquipment.gameObject.SetActive(true);
        }

    }

    public void TurnBuff()
    {
        if (!containerBuff.gameObject.activeSelf)
        {
            ResetSubPopUps();
            containerBuff.gameObject.SetActive(true);

            foreach (var item in buffGridviewItems)
            {
                item.ResetViews();
            }

            for (int i = 0; i < InventoryController.Instance.GetPlayerData.Buffs.Count; i++)
            {
                var buff = InventoryController.Instance.GetPlayerData.Buffs[i];

                var gridview = buffGridviewItems[i];

                gridview.UpdateViews(buff);
            }
        }
        else
        {
            ResetSubPopUps();
            containerEquipment.gameObject.SetActive(true);
        }
    }
}
