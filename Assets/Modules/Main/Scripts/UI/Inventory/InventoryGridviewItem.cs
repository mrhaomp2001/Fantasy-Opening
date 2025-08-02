using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryGridviewItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textItemCount;
    [SerializeField] private Image imageItem;
    public void UpdateViews(string count, Sprite spriteItem)
    {
        textItemCount.text = count;
        imageItem.sprite = spriteItem;
    }
}
