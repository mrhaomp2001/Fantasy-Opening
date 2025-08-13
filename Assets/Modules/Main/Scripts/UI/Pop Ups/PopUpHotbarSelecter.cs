using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpHotbarSelecter : PopUp
{
    private static PopUpHotbarSelecter instance;
    private InventoryController.InventoryItem currentSelectItem;
    [Header("Hotbar Selecter: ")]
    [SerializeField] private List<HotbarSelecterItem> hotbarSelecterItems;

    public static PopUpHotbarSelecter Instance { get => instance; set => instance = value; }

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

    public void ShowPopUp(InventoryController.InventoryItem valueItem)
    {
        base.Show();
        currentSelectItem = valueItem;

        for (int i = 0; i < hotbarSelecterItems.Count; i++)
        {
            HotbarSelecterItem item = hotbarSelecterItems[i];

            if (InventoryController.Instance.GetPlayerData.Hotbar[i].item != null)
            {
                item.UpdateViews(InventoryController.Instance.GetPlayerData.Hotbar[i]);
            }
            else
            {
                item.UpdateViews(null);
            }
        }
    }

    public void SelectHotbarSlot(int slot)
    {
        InventoryController.Instance.SelectHotbarSlot(slot, currentSelectItem);
        Hide();
    }
}
