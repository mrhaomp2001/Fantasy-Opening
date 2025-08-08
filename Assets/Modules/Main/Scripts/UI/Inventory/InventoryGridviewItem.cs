using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryGridviewItem : MonoBehaviour
{
    [SerializeField] private Sprite mask;
    [SerializeField] private InventoryController.InventoryItem item;
    [SerializeField] private TextMeshProUGUI textItemCount;
    [SerializeField] private Image imageItem;
    public void UpdateViews(InventoryController.InventoryItem valueItem)
    {
        if (valueItem != null)
        {
            item = valueItem;
            if (item.item != null)
            {
                textItemCount.text = valueItem.count.ToString();
                imageItem.sprite = valueItem.item.Sprite;
            }
            else
            {
                imageItem.sprite = mask;
            }

        }
    }

    public void RefreshItem()
    {
        item = null;
        textItemCount.text = "";
    }

    public void OnClick()
    {
        if (item == null)
        {
            return;
        }
        if (item.item == null)
        {
            return;
        }

        if (PopUpInventory.Instance.IsOpeningChest)
        {
            if (PopUpInventory.Instance.CurrentChest.Add(item.item.Id, item.count))
            {
                InventoryController.Instance.Consume(item.item.Id, item.count, new Callback()
                {
                    onSuccess = () =>
                    {
                    },
                    onFail = (message) =>
                    {
                        Debug.LogWarning(message);
                    },
                    onNext = () =>
                    {
                        PopUpInventory.Instance.UpdateViews();
                    }
                });
            }
        }
        else
        {
            PopUpHotbarSelecter.Instance.ShowPopUp(item);
        }

    }

    public void OnClickFromChest()
    {
        if (item == null)
        {
            return;
        }
        if (item.item == null)
        {
            return;
        }

        if (InventoryController.Instance.Add(item.item.Id, item.count))
        {
            PopUpInventory.Instance.CurrentChest.Consume(item.item.Id, item.count, new Callback()
            {
                onSuccess = () =>
                {

                },
                onFail = (message) =>
                {
                    Debug.LogWarning(message);
                },
                onNext = () =>
                {
                    PopUpInventory.Instance.UpdateViews();

                }
            });
        }
    }
}
