using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static InventoryController;

[System.Serializable, JsonObject(MemberSerialization.OptIn)]
public class BuildingChest : BuildingBase, IPoolObject
{
    [JsonProperty]
    [SerializeField] private List<InventoryController.InventoryItem> items = new();
    [SerializeField] private SpriteRenderer spriteRenderer;

    public List<InventoryController.InventoryItem> Items { get => items; set => items = value; }
    public bool IsChestFull
    {
        get
        {
            return Items.All(i => i.item != null);
        }
    }


    public bool Add(int id, int count)
    {
        InventoryItem item = items.Where((item) => { return (item.item != null && item.item.Id == id); }).FirstOrDefault();
        if (item != null)
        {
            item.count += count;

            return true;
        }

        if (IsChestFull)
        {
            return false;
        }

        var targetItem = ItemDatabase.Instance.Items
            .Where(predicate =>
            {
                return predicate.Id == id;
            })
            .FirstOrDefault();

        if (targetItem != null)
        {
            var inventorySlot = items
                .Where(predicate =>
                {
                    return predicate.item == null;
                })
                .FirstOrDefault();

            if (inventorySlot != null)
            {
                inventorySlot.item = targetItem;
                inventorySlot.count = count;
            }

        }
        return true;
    }
    public void Consume(int id, int count, Callback callback)
    {
        InventoryItem item = items.Where((item) => { return (item.item != null && item.item.Id == id); }).FirstOrDefault();
        if (item != null)
        {
            if (item.count >= count)
            {
                item.count -= count;
                if (item.count <= 0)
                {
                    item.item = null;
                }
                callback?.onSuccess?.Invoke();
                callback?.onNext?.Invoke();
            }
            else
            {
                callback?.onFail?.Invoke("Not enough items in chest.");
                callback?.onNext?.Invoke();

                Debug.LogWarning("Not enough items in chest to consume.");
            }
        }
        else
        {
            callback?.onFail?.Invoke("Item not found in chest.");
            callback?.onNext?.Invoke();
            Debug.LogWarning("Item not found in chest.");
        }
    }
    public void OnObjectSpawnAfter()
    {

        spriteRenderer.sortingOrder = (int)-(transform.position.y * 100f);
        items = new();

        for (int i = 0; i < 9; i++)
        {
            items.Add(new());
        }
    }

    public override void OnWorldInteract()
    {
        PopUpInventory.Instance.TurnPopUp(this);

        AudioController.Instance.Play("chest_open");
    }

}
