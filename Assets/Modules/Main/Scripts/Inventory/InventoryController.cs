using Newtonsoft.Json;
using SimpleJSON;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

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

        public List<InventoryItem> Items { get => items; set => items = value; }
        public List<InventoryItem> Hotbar { get => hotbar; set => hotbar = value; }
        public int HotbarSelectedSlot { get => hotbarSelectedSlot; set => hotbarSelectedSlot = value; }

        public InventoryItem SelectedHotbar
        {
            get
            {
                return Hotbar[HotbarSelectedSlot];
            }
        }
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

        OnLoadPrefs();

        BuildingController.Instance.Load();
    }

    public void Consume(int id, int count, Callback callback)
    {
        InventoryItem item = playerData.Items.Where((item) => { return item.item.Id == id; }).FirstOrDefault();

        if (item == null)
        {
            //callback.onFail?.Invoke("Không sở hữu item này");
            //callback.onNext?.Invoke();

            item = playerData.Hotbar.Where((item) => { return (item.item != null && item.item.Id == id); }).FirstOrDefault();
        }

        if (item == null)
        {
            callback.onFail?.Invoke("Không sở hữu item này");
            callback.onNext?.Invoke();
        }

        if (item.count < count)
        {
            callback.onFail?.Invoke("Không đủ số lượng " + item.item.ItemName);

            callback.onNext?.Invoke();


            PopUpInventory.Instance.UpdateViewHotbar();
            //PopUpInventory.Instance.UpdateViews();

            return;
        }

        item.count -= count;

        if (item.count <= 0)
        {
            playerData.Items.Remove(item);

            item.item = null;
            item.count = 0;
        }

        Debug.Log("Tiêu thụ thành công nha!");

        callback.onSuccess?.Invoke();
        callback.onNext?.Invoke();

        PopUpInventory.Instance.UpdateViewHotbar();
        //PopUpInventory.Instance.UpdateViews();
    }

    public void Add(int id, int count, bool isNonCheckHotbar = false)
    {
        InventoryItem item = playerData.Items.Where((item) => { return item.item.Id == id; }).FirstOrDefault();
        if (item != null)
        {
            item.count += count;


            PopUpInventory.Instance.UpdateViewHotbar();
            //PopUpInventory.Instance.UpdateViews();

            return;
        }

        if (!isNonCheckHotbar)
        {
            item = playerData.Hotbar.Where((item) => { return (item.item != null && item.item.Id == id); }).FirstOrDefault();

            if (item != null)
            {
                item.count += count;


                PopUpInventory.Instance.UpdateViewHotbar();
                //PopUpInventory.Instance.UpdateViews();

                return;
            }
        }

        var targetItem = ItemDatabase.Instance.Items
            .Where(predicate =>
            {
                return predicate.Id == id;
            })
            .FirstOrDefault();

        if (targetItem != null)
        {
            playerData.Items
            .Add(
                new InventoryItem
                {
                    count = count,
                    item = targetItem
                }
            );
        }
        else
        {
            Debug.Log("Don't have item with id: " + id);
        }

        PopUpInventory.Instance.UpdateViewHotbar();
        //PopUpInventory.Instance.UpdateViews();
    }

    public void OnLoadPrefs()
    {
        playerData.Hotbar = new();
        playerData.Items = new();

        playerData.Hotbar.Add(new());// 1
        playerData.Hotbar.Add(new());// 2
        playerData.Hotbar.Add(new());// 3
        playerData.Hotbar.Add(new());// 4
        playerData.Hotbar.Add(new());// 5
        playerData.Hotbar.Add(new());// 6
        playerData.Hotbar.Add(new());// 7
        playerData.Hotbar.Add(new());// 8
        playerData.Hotbar.Add(new());// 9

        if (PlayerPrefs.HasKey(prefKey))
        {
            string value = PlayerPrefs.GetString(prefKey);

            JSONNode keyValuePairs = JSONNode.Parse(value);

            Debug.Log($"OnLoadPrefs: {value}");

            // items
            for (int i = 0; i < keyValuePairs["items"].Count; i++)
            {
                var item = keyValuePairs["items"][i];
                Add(item["item"]["id"].AsInt, item["count"].AsInt);
            }

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


        }

        PopUpInventory.Instance.UpdateViewHotbar();

        OnSavePrefs();
    }

    public void SelectHotbarSlot(int slot, InventoryController.InventoryItem itemTarget)
    {
        if (playerData.Hotbar[slot].item != null)
        {
            Add(playerData.Hotbar[slot].item.Id, playerData.Hotbar[slot].count, isNonCheckHotbar: true);
        }

        playerData.Hotbar[slot] = itemTarget;

        playerData.Items.Remove(itemTarget);

        PopUpInventory.Instance.UpdateViewHotbar();
        PopUpInventory.Instance.UpdateViews();
    }

    public void DeselectHotbarSlot(int slot, InventoryController.InventoryItem itemTarget)
    {
        if (itemTarget.item == null)
        {
            return;
        }
        Add(itemTarget.item.Id, itemTarget.count, isNonCheckHotbar: true);

        GetPlayerData.Hotbar[slot].item = null;
        GetPlayerData.Hotbar[slot].count = 0;

        PopUpInventory.Instance.UpdateViews();
        PopUpInventory.Instance.UpdateViewHotbar();
    }

    public void OnSavePrefs()
    {
        Debug.Log($"OnSavePrefs: {JsonConvert.SerializeObject(playerData)}");
        PlayerPrefs.SetString(prefKey, JsonConvert.SerializeObject(playerData));
        PlayerPrefs.Save();
    }
}
