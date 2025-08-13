using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCraftingTooltipItem : MonoBehaviour
{
    [SerializeField] private Image imageSpriteItem;
    [SerializeField] private TextMeshProUGUI textItemName;
    [SerializeField] private TextMeshProUGUI textItemCount;

    public void UpdateViews(Sprite sprite, string name, string count)
    {
        imageSpriteItem.sprite = sprite;
        textItemName.text = name;
        textItemCount.text = count;
    }
}
