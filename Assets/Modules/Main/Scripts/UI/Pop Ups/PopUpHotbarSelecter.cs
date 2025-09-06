using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class PopUpHotbarSelecter : PopUp
{
    private static PopUpHotbarSelecter instance;
    private InventoryController.InventoryItem currentSelectItem;
    [Header("Hotbar Selecter: ")]
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI textName, textDescription;
    [SerializeField] private RectTransform containerEquipEquipment;
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

        containerEquipEquipment.gameObject.SetActive(false);

        itemImage.sprite = currentSelectItem.item.Sprite;
        textName.SetText(currentSelectItem.item.ItemName);
        textDescription.SetText(currentSelectItem.item.ItemDescription);

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

        if (valueItem.item is ItemArmorHead || valueItem.item is ItemArmorBody || valueItem.item is ItemArmorLeg || valueItem.item is ItemArmorFoot)
        {
            containerEquipEquipment.gameObject.SetActive(true);
        }
    }

    public void OnEquipEquipment()
    {
        InventoryController.Instance.EquipEquipment(currentSelectItem.item);
        Hide();
    }

    public void SelectHotbarSlot(int slot)
    {
        InventoryController.Instance.SelectHotbarSlot(slot, currentSelectItem);
        Hide();
    }

    public void OnThrowItem()
    {
        int itemId = currentSelectItem.item.Id;
        int itemCount = currentSelectItem.count;
        InventoryController.Instance.Consume(currentSelectItem.item.Id, currentSelectItem.count, new Callback
        {
            onSuccess = () =>
            {
                WorldItemController.Instance.SpawnItem(itemId, PlayerController.Instance.RbPlayer.position, itemCount);
            },
            onFail = (message) =>
            {

            },
            onNext = () =>
            {
                Hide();
                PopUpInventory.Instance.UpdateViews();
            }
        });
    }

    public void OnDeleteItem()
    {
        InventoryController.Instance.Consume(currentSelectItem.item.Id, currentSelectItem.count, new Callback
        {
            onSuccess = () =>
            {

            },
            onFail = (message) =>
            {

            },
            onNext = () =>
            {
                Hide();
                PopUpInventory.Instance.UpdateViews();
            }
        });
    }
}
