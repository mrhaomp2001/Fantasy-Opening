using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HotbarItem : MonoBehaviour
{
    [SerializeField] private int slot;
    [SerializeField] private Sprite spriteMask;
    [SerializeField] private Image imageItem;
    [SerializeField] private Image imageSelected;
    [SerializeField] private TextMeshProUGUI textCount;
    [SerializeField] private RectTransform tooltipPosition;

    private InventoryController.InventoryItem item;

    [SerializeField] private List<RectTransform> qualityHolders;

    private void UpdateQuality()
    {
        foreach (var qualityHolder in qualityHolders)
        {
            qualityHolder.gameObject.SetActive(false);
        }

        switch (item.item.Quality)
        {
            case ItemQuality.Normal:
                qualityHolders[0].gameObject.SetActive(true);
                break;
            case ItemQuality.Common:
                qualityHolders[1].gameObject.SetActive(true);
                break;
            case ItemQuality.Rare:
                qualityHolders[2].gameObject.SetActive(true);
                break;
            case ItemQuality.SuperRare:
                qualityHolders[3].gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
    public void UpdateViews(InventoryController.InventoryItem targetItem)
    {
        imageItem.sprite = spriteMask;
        textCount.SetText("");
        item = targetItem;
        if (item != null)
        {
            if (item.item != null)
            {
                imageItem.sprite = item.item.Sprite;
                if (!item.item.IsNonStack)
                {
                    textCount.SetText(item.count.ToString());
                }
                else
                {
                    textCount.SetText(string.Empty);
                }

                UpdateQuality();
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
        if (FishingController.Instance.IsFishing)
        {
            return;
        }

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

    public void OnPointerEnter(BaseEventData baseEventData)
    {
        if (baseEventData is PointerEventData pointerEventData)
        {
            PopUpInventoryTooltip.Instance.ShowAtPosition(tooltipPosition.position, item, new Vector2(0f, 0f));
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
