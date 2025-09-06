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

    public void UpdateViews(InventoryController.InventoryItem valueItemBase)
    {
        imageItem.sprite = spriteMask;
        textCount.SetText("");

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
        }
    }
    public void OnClick()
    {
        PopUpHotbarSelecter.Instance.SelectHotbarSlot(slot);
    }
}
