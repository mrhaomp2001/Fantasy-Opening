using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryGridviewItem : MonoBehaviour
{
    [SerializeField] private Sprite mask;
    [SerializeField] private InventoryController.InventoryItem item;
    [SerializeField] private TextMeshProUGUI textItemCount;
    [SerializeField] private Image imageItem;
    [SerializeField] private RectTransform tooltipPosition;
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
        imageItem.sprite = mask;

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
        else if (PopUpInventory.Instance.IsSelling)
        {
            if (item.count > 0)
            {

                var itemTarget = PopUpInventory.Instance.SellingGridviewItems
                    .Where(predicate =>
                    {
                        return predicate.item != null && predicate.item.item.Id == item.item.Id;
                    })
                    .FirstOrDefault();

                if (itemTarget != null && InventoryController.Instance.Add(407, item.item.SellPrice))
                {
                    InventoryController.Instance.Consume(item.item.Id, 1, new Callback()
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
                            PopUpInventoryTooltip.Instance.ShowAtPosition(tooltipPosition.position, item, new Vector2(0f, 0.5f));
                        }
                    });
                }
                else
                {
                    Debug.Log("Item not found in selling gridview or unable to add gold.");
                }
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

    public void OnClickUnequip() 
    {
        if (item != null)
        {
            if (item.item != null)
            {
                if(InventoryController.Instance.Add(item.item.Id, 1))
                {
                    if (item.item is ItemArmorHead)
                    {
                        InventoryController.Instance.GetPlayerData.ArmorHead.item = null;
                    }
                    if (item.item is ItemArmorBody)
                    {
                        InventoryController.Instance.GetPlayerData.ArmorBody.item = null;

                    }
                    if (item.item is ItemArmorLeg)
                    {
                        InventoryController.Instance.GetPlayerData.ArmorLeg.item = null;

                    }
                    if (item.item is ItemArmorFoot)
                    {
                        InventoryController.Instance.GetPlayerData.ArmorFoot.item = null;
                    }

                    PopUpInventory.Instance.UpdateViews();
                    PopUpInventoryTooltip.Instance.ShowAtPosition(tooltipPosition.position, item, new Vector2(0f, 0.5f));
                }
            }
        }
    }

    public void OnPointerEnter(BaseEventData baseEventData)
    {
        if (baseEventData is PointerEventData pointerEventData)
        {
            PopUpInventoryTooltip.Instance.ShowAtPosition(tooltipPosition.position, item, new Vector2(0f, 0.5f));
        }
    }

    public void OnPointerExit(BaseEventData baseEventData)
    {
        if (baseEventData is PointerEventData pointerEventData)
        {
            PopUpInventoryTooltip.Instance.Hide();
        }
    }
}
