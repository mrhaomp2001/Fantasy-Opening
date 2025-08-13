using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpInventoryTooltip : PopUp
{
    private static PopUpInventoryTooltip instance;

    private InventoryController.InventoryItem targetItem;
    [Header("Inventory Tooltip: ")]

    [SerializeField] private TextMeshProUGUI textItemName;
    [SerializeField] private TextMeshProUGUI textItemDescription;
    [SerializeField] private TextMeshProUGUI textItemPrice;
    [SerializeField] private Image imageItemSprite;

    public static PopUpInventoryTooltip Instance { get => instance; set => instance = value; }
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


    private void Start()
    {
        Hide();
    }

    public void ShowAtPosition(Vector2 position, InventoryController.InventoryItem item)
    {
        if (item.item != null)
        {
            base.Show();
            container.position = position + new Vector2(5f, 0f);
            targetItem = item;

            textItemName.text = targetItem.item.ItemName;
            textItemDescription.text = targetItem.item.ItemDescription;
            textItemPrice.text = $"{LanguageController.Instance.GetString("value_sell")}: {targetItem.item.SellPrice}";
            imageItemSprite.sprite = item.item.Sprite;
        }

    }
}
