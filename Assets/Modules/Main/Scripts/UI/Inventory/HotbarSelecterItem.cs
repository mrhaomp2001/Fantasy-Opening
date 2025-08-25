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
        textCount.text = "";

        if (valueItemBase != null)
        {
            imageItem.sprite = valueItemBase.item.Sprite;
            if (valueItemBase.item.IsNonStack)
            {
                textCount.text = "";

            }
            else
            {
                textCount.text = valueItemBase.count.ToString();
            }
        }
    }
    public void OnClick()
    {
        PopUpHotbarSelecter.Instance.SelectHotbarSlot(slot);
    }
}
