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
        public int id;
        public int count;
        [JsonIgnore] public ItemBase item;
    }
    [JsonObject(MemberSerialization.OptIn), System.Serializable]
    public class PlayerData
    {
        [JsonProperty][SerializeField] private int weaponId;
        [JsonProperty][SerializeField] private List<InventoryItem> items = new();

        public int WeaponId { get => weaponId; set => weaponId = value; }
        public List<InventoryItem> Items { get => items; set => items = value; }
    }

    private static InventoryController instance;
    private const string prefKey = "InventoryController";

    [SerializeField] private int money;
    [SerializeField] private PlayerData playerData;


    [Header("Equipments: ")]
    [SerializeField] private ItemBase itemWeapon;
    public ItemBase ItemWeapon { get => itemWeapon; set => itemWeapon = value; }

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
    }

    public void Consume(int id, int count, Callback callback)
    {
        InventoryItem item = playerData.Items.Where((item) => { return item.item.Id == id; }).FirstOrDefault();

        if (item == null)
        {
            callback.onFail?.Invoke("Không sở hữu item này");
            callback.onNext?.Invoke();

            return;

        }

        if (item.count < count)
        {
            callback.onFail?.Invoke("Không đủ số lượng " + item.item.ItemName);

            callback.onNext?.Invoke();
            return;
        }

        item.count -= count;

        if (item.count <= 0)
        {
            playerData.Items.Remove(item);
        }

        Debug.Log("Tiêu thụ thành công nha!");

        callback.onSuccess?.Invoke();
        callback.onNext?.Invoke();
    }

    public void Add(int id, int count)
    {
        InventoryItem item = playerData.Items.Where((item) => { return item.id == id; }).FirstOrDefault();

        if (item == null)
        {
            var targetItem = ItemDatabase.Instance.Items
                .Where(predicate =>
                {
                    return predicate.Id == id;
                })
                .FirstOrDefault();

            playerData.Items
                .Add(
                new InventoryItem
                {
                    id = id,
                    count = count,
                    item = targetItem
                });
        }
        else
        {
            item.count += count;

        }
    }

    public void OnLoadPrefs()
    {
        if (PlayerPrefs.HasKey(prefKey))
        {
            string value = PlayerPrefs.GetString(prefKey);

            JSONNode keyValuePairs = JSONNode.Parse(value);

            // items
            for (int i = 0; i < keyValuePairs["items"].Count; i++)
            {
                var item = keyValuePairs["items"][i];
                Add(item["id"].AsInt, item["count"].AsInt);
            }

            // weapon current
            if (keyValuePairs["weaponId"].AsInt != 0)
            {
                Add(keyValuePairs["weaponId"].AsInt, 1);

                PopUpInventory.Instance.EquipWeapon(playerData.Items
                    .Where(predicate =>
                    {
                        return predicate.item.Id == keyValuePairs["weaponId"].AsInt;
                    })
                    .FirstOrDefault()
                    .item);
            }


        }

        OnSavePrefs();
    }

    public void OnSavePrefs()
    {
        if (itemWeapon != null)
        {
            playerData.WeaponId = itemWeapon.Id;
        }
        else
        {
            playerData.WeaponId = 0;
        }

        PlayerPrefs.SetString(prefKey, JsonConvert.SerializeObject(playerData));
        PlayerPrefs.Save();
    }
}
