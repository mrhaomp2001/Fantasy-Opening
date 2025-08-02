using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpInventory : PopUp
{
    private static PopUpInventory instance;

    [Header("PopUpInventory: ")]
    [SerializeField] private GameObject prefabInventoryItem;
    [SerializeField] private RectTransform contentInventory;

    public static PopUpInventory Instance { get => instance; set => instance = value; }

    private void Start()
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

    public void TurnPopUp()
    {
        foreach (Transform item in contentInventory.transform)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in InventoryController.Instance.Items)
        {
            var targetObject = Instantiate(prefabInventoryItem, contentInventory.transform);

            var gridviewItem = targetObject.GetComponent<InventoryGridviewItem>();

            gridviewItem.UpdateViews(item.count.ToString(), item.item.Sprite);
        }

        base.Turn();
    }
}
