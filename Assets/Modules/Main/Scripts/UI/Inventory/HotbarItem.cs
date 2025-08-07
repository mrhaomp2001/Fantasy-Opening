using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotbarItem : MonoBehaviour
{
    [SerializeField] private int slot;
    [SerializeField] private Sprite spriteMask;
    [SerializeField] private Image imageItem;
    [SerializeField] private Image imageSelected;
    [SerializeField] private TextMeshProUGUI textCount;

    private InventoryController.InventoryItem item;

    public void UpdateViews(InventoryController.InventoryItem targetItem)
    {
        imageItem.sprite = spriteMask;
        textCount.text = "";
        item = targetItem;
        if (item != null)
        {
            if (item.item != null)
            {
                imageItem.sprite = item.item.Sprite;
                textCount.text = item.count.ToString();
            }
        }

        UpdateSelectedState();
    }

    public void UpdateSelectedState()
    {
        imageSelected.gameObject.SetActive(false);

        if (slot == InventoryController.Instance.GetPlayerData.HotbarSelectedSlot)
        {
            imageSelected.gameObject.SetActive(true);
        }
    }

    public void OnClick()
    {
        if (PopUpInventory.Instance.IsOpening)
        {
            if (item != null)
            {
                InventoryController.Instance.DeselectHotbarSlot(slot, item);
            }
        }
        else
        {
            InventoryController.Instance.GetPlayerData.HotbarSelectedSlot = slot;
            PopUpInventory.Instance.UpdateViewHotbar();
        }
    }
}
