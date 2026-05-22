using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSelecterItem : MonoBehaviour
{
    [SerializeField] private int slot;
    [SerializeField] private Sprite spriteMask;
    [SerializeField] private Image imageItem;
    [SerializeField] private TextMeshProUGUI textCount;

    [SerializeField] private List<RectTransform> qualityHolders;

    private InventoryController.InventoryItem item;

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
    public void UpdateViews(InventoryController.InventoryItem valueItemBase)
    {
        imageItem.sprite = spriteMask;
        textCount.SetText("");
        item = valueItemBase;
        if (valueItemBase != null)
        {
            imageItem.sprite = valueItemBase.item.Sprite;
            if (valueItemBase.item.IsNonStack)
            {
                textCount.SetText("");

            }
            else
            {
                textCount.SetText(valueItemBase.count.ToString());
            }

            UpdateQuality();
        }
    }
    public void OnClick()
    {
        PopUpHotbarSelecter.Instance.SelectHotbarSlot(slot);
    }
}
