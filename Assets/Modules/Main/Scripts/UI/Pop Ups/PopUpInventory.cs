using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpInventory : PopUp
{
    private static PopUpInventory instance;

    [Header("PopUpInventory: ")]
    [SerializeField] private GameObject prefabInventoryItem;
    [SerializeField] private RectTransform contentInventory;
    [Header("--: ")]
    [SerializeField] private InventoryWeaponSlot weaponSlot;

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
        if (!container.gameObject.activeSelf)
        {
            UpdateViews();
        }

        base.Turn();
    }

    private void UpdateViews()
    {
        foreach (Transform item in contentInventory.transform)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in InventoryController.Instance.GetPlayerData.Items)
        {
            var targetObject = Instantiate(prefabInventoryItem, contentInventory.transform);

            var gridviewItem = targetObject.GetComponent<InventoryGridviewItem>();

            gridviewItem.UpdateViews(item);
        }

        weaponSlot.UpdateViews();
    }

    public void EquipWeapon(ItemBase weapon)
    {
        if (InventoryController.Instance.ItemWeapon != null)
        {
            UnequipWeapon();
        }

        InventoryController.Instance.Consume(weapon.Id, 1, new Callback
        {
            onSuccess = () =>
            {
                InventoryController.Instance.ItemWeapon = weapon;

                weaponSlot.UpdateViews();
                UpdateViews();

            },
            onFail = (message) =>
            {

            },
            onNext = () =>
            {

            }
        });
    }

    public void UnequipWeapon()
    {
        Debug.Log("UnequipWeapon");
        if (InventoryController.Instance.ItemWeapon == null)
        {
            return;
        }

        InventoryController.Instance.Add(InventoryController.Instance.ItemWeapon.Id, 1);

        InventoryController.Instance.ItemWeapon = null;

        weaponSlot.UpdateViews();
        UpdateViews();
    }
}
