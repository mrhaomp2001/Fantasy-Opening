using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryGridviewItem : MonoBehaviour
{
    private InventoryController.InventoryItem item;
    [SerializeField] private TextMeshProUGUI textItemCount;
    [SerializeField] private Image imageItem;
    public void UpdateViews(InventoryController.InventoryItem valueItem)
    {
        gameObject.SetActive(true);

        item = valueItem;

        textItemCount.text = valueItem.count.ToString();
        imageItem.sprite = valueItem.item.Sprite;
    }

    public void RefreshItem()
    {
        if (gameObject.activeSelf)
        {
            item = null;
            gameObject.SetActive(false);
            textItemCount.text = "";
        }
    }

    public void OnClick()
    {
        PopUpHotbarSelecter.Instance.ShowPopUp(item);
    }
}
