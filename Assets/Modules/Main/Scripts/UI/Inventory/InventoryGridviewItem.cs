using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryGridviewItem : MonoBehaviour
{
    private InventoryController.InventoryItem item;
    [SerializeField] private TextMeshProUGUI textItemCount;
    [SerializeField] private Image imageItem;
    public void UpdateViews(InventoryController.InventoryItem valueItem)
    {
        item = valueItem;

        textItemCount.text = valueItem.count.ToString();
        imageItem.sprite = valueItem.item.Sprite;
    }

    public void OnClick()
    {
        if (item.item is ItemBase weapon)
        {
            PopUpInventory.Instance.EquipWeapon(weapon);
        }
    }
}
