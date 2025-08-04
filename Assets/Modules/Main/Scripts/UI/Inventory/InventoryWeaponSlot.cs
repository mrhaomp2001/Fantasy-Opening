using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryWeaponSlot : MonoBehaviour
{
    [SerializeField] private Sprite spriteDefault;
    [SerializeField] private Image imageWeapon;
    public void UpdateViews()
    {
        if (InventoryController.Instance.ItemWeapon == null)
        {
            imageWeapon.sprite = spriteDefault;
        }
        else
        {
            imageWeapon.sprite = InventoryController.Instance.ItemWeapon.Sprite;
        }

    }
    public void OnClick()
    {
        PopUpInventory.Instance.UnequipWeapon();
    }
}
