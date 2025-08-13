using Newtonsoft.Json;
using SimpleJSON;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [System.Serializable]
    public class InventoryItem
    {
        public int count;
        public ItemBase item;
    }
    [JsonObject(MemberSerialization.OptIn), System.Serializable]
    public class PlayerData
    {
        [JsonProperty]
        [SerializeField] private List<InventoryItem> items = new();

        [JsonProperty]
        [SerializeField] private List<InventoryItem> hotbar = new();

        [JsonProperty]
        [SerializeField] private int hotbarSelectedSlot;
        [JsonProperty]
        [SerializeField] private BuildingController.BuildingData buildingData;

        public List<InventoryItem> Items { get => items; set => items = value; }
        public List<InventoryItem> Hotbar { get => hotbar; set => hotbar = value; }
        public int HotbarSelectedSlot { get => hotbarSelectedSlot; set => hotbarSelectedSlot = value; }

        public bool IsInventoryFull
        {
            get
            {
                return Items.All(i => i.item != null);
            }
        }

        public bool IsItemEnough(int id, int count)
        {
            InventoryItem item = Items.Where((item) => { return (item.item != null && item.item.Id == id); }).FirstOrDefault();

            if (item == null)
            {
                item = Hotbar.Where((item) => { return (item.item != null && item.item.Id == id); }).FirstOrDefault();
            }

            if (item == null || item.count < count)
            {
                return false;
            }

            return true;
        }

        public int CheckItemCount(int id)
        {
            InventoryItem item = Items.Where((item) => { return (item.item != null && item.item.Id == id); }).FirstOrDefault();

            if (item == null)
            {
                item = Hotbar.Where((item) => { return (item.item != null && item.item.Id == id); }).FirstOrDefault();
            }

            if (item == null)
            {
                return 0;
            }

            return item.count;
        }

        public InventoryItem SelectedHotbar
        {
            get
            {
                return Hotbar[HotbarSelectedSlot];
            }
        }

        public BuildingController.BuildingData BuildingData { get => buildingData; set => buildingData = value; }
    }

    private static InventoryController instance;
    private const string prefKey = "InventoryController";

    [SerializeField] private int money;
    [SerializeField] private PlayerData playerData;

    public static InventoryController Instance { get => instance; set => instance = value; }
    public int Money
    {
        get => money; set
        {
            money = value;
        }
    }

    public PlayerData GetPlayerData { get => playerData; set => playerData = value; }

    private void Awake()
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

    private void Start()
    {
        playerData = new PlayerData();

        Load();

    }

    public void Consume(int id, int count, Callback callback)
    {
        InventoryItem item = playerData.Items.Where((item) => { return (item.item != null && item.item.Id == id); }).FirstOrDefault();

        if (item == null)
        {
            item = playerData.Hotbar.Where((item) => { return (item.item != null && item.item.Id == id); }).FirstOrDefault();
        }

        if (item == null)
        {
            callback.onFail?.Invoke("Không sở hữu item này");
            callback.onNext?.Invoke();

            return;
        }

        if (item.count < count)
        {
            callback.onFail?.Invoke("Không đủ số lượng ");

            callback.onNext?.Invoke();

            PopUpInventory.Instance.UpdateViewHotbar();

            return;
        }

        item.count -= count;

        if (item.count <= 0)
        {
            item.item = null;
            item.count = 0;
        }

        Debug.Log("Tiêu thụ thành công nha!");

        callback.onSuccess?.Invoke();
        callback.onNext?.Invoke();

        PopUpInventory.Instance.UpdateViewHotbar();
        //PopUpInventory.Instance.UpdateViews();
    }

    public bool Add(int id, int count, bool isNonCheckHotbar = false)
    {
        InventoryItem item = playerData.Items.Where((item) => { return (item.item != null && item.item.Id == id); }).FirstOrDefault();
        if (item != null)
        {
            item.count += count;


            PopUpInventory.Instance.UpdateViewHotbar();
            //PopUpInventory.Instance.UpdateViews();

            return true;
        }

        if (!isNonCheckHotbar)
        {
            item = playerData.Hotbar.Where((item) => { return (item.item != null && item.item.Id == id); }).FirstOrDefault();

            if (item != null)
            {
                item.count += count;


                PopUpInventory.Instance.UpdateViewHotbar();
                //PopUpInventory.Instance.UpdateViews();

                return true;
            }
        }


        if (playerData.IsInventoryFull)
        {
            Debug.Log("Inventory is full, cannot add item with id: " + id);
            PopUpInventory.Instance.UpdateViewHotbar();

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
            var inventorySlot = playerData.Items
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
        else
        {
            Debug.Log("Don't have item with id: " + id);
        }

        PopUpInventory.Instance.UpdateViewHotbar();
        //PopUpInventory.Instance.UpdateViews();

        return true;
    }

    public void Load()
    {
        playerData.Hotbar = new();
        playerData.Items = new();

        for (int i = 0; i < 27; i++)
        {
            playerData.Items.Add(new());// i
        }

        for (int i = 0; i < 9; i++)
        {
            playerData.Hotbar.Add(new());// i
        }

        playerData.BuildingData = new BuildingController.BuildingData
        {
            IdCounter = 0,
            Buildings = new List<BuildingController.Building>()
        };


        if (PlayerPrefs.HasKey(prefKey))
        {
            string value = PlayerPrefs.GetString(prefKey);

            JSONNode keyValuePairs = JSONNode.Parse(value);

            Debug.Log($"OnLoadPrefs: {value}");

            // hotbar
            playerData.HotbarSelectedSlot = keyValuePairs["hotbarSelectedSlot"].AsInt;

            for (int i = 0; i < keyValuePairs["hotbar"].Count; i++)
            {
                var item = keyValuePairs["hotbar"][i];

                if (item != null)
                {
                    if (item["item"] != null)
                    {
                        if (item["item"]["id"].AsInt != 0)
                        {
                            Add(item["item"]["id"].AsInt, item["count"].AsInt);

                            SelectHotbarSlot(i, playerData.Items
                                .Where(predicate =>
                                {
                                    return predicate.item.Id == item["item"]["id"].AsInt;
                                })
                                .FirstOrDefault());
                        }
                    }

                }
            }

            // items
            for (int i = 0; i < keyValuePairs["items"].Count; i++)
            {
                var item = keyValuePairs["items"][i];
                if (item["item"] != null)
                {
                    Add(item["item"]["id"].AsInt, item["count"].AsInt);
                }
            }

            BuildingController.Instance.Load(keyValuePairs["buildingData"]);

        }

        PopUpInventory.Instance.UpdateViewHotbar();

        Save();
    }

    public void SelectHotbarSlot(int slot, InventoryController.InventoryItem itemTarget)
    {
        int itemId = 0, itemCount = 0;

        if (playerData.Hotbar[slot].item != null)
        {
            itemId = playerData.Hotbar[slot].item.Id;
            itemCount = playerData.Hotbar[slot].count;
        }

        playerData.Hotbar[slot] = itemTarget;

        playerData.Items.Remove(itemTarget);
        playerData.Items.Add(new InventoryItem());

        if (itemId != 0)
        {
            Add(itemId, itemCount, isNonCheckHotbar: true);
        }

        PopUpInventory.Instance.UpdateViewHotbar();
        PopUpInventory.Instance.UpdateViews();
    }

    public void DeselectHotbarSlot(int slot, InventoryController.InventoryItem itemTarget)
    {
        if (itemTarget.item == null)
        {
            return;
        }

        if (Add(itemTarget.item.Id, itemTarget.count, isNonCheckHotbar: true))
        {
            GetPlayerData.Hotbar[slot].item = null;
            GetPlayerData.Hotbar[slot].count = 0;
        }



        PopUpInventory.Instance.UpdateViews();
        PopUpInventory.Instance.UpdateViewHotbar();
    }

    public void Save()
    {
        BuildingController.Instance.Save();

        Debug.Log($"OnSavePrefs: {JsonConvert.SerializeObject(playerData)}");
        PlayerPrefs.SetString(prefKey, JsonConvert.SerializeObject(playerData));
        PlayerPrefs.Save();
    }
}
