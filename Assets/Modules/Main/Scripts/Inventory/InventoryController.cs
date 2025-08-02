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

    private static InventoryController instance;
    private const string prefKey = "InventoryController";

    [SerializeField] private int money;
    [SerializeField] private List<InventoryItem> items;

    public static InventoryController Instance { get => instance; set => instance = value; }
    public List<InventoryItem> Items { get => items; set => items = value; }
    public int Money
    {
        get => money; set
        {
            money = value;
        }
    }

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
        OnLoadPrefs();
        OnSavePrefs();
    }

    public void Consume(int id, int count, Callback callback)
    {
        InventoryItem item = items.Where((item) => { return item.item.Id == id; }).FirstOrDefault();

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
            items.Remove(item);
        }

        Debug.Log("Tiêu thụ thành công nha!");

        callback.onSuccess?.Invoke();
        callback.onNext?.Invoke();
    }

    public void Add(int id, int count)
    {
        InventoryItem item = items.Where((item) => { return item.id == id; }).FirstOrDefault();

        if (item == null)
        {
            var targetItem = ItemDatabase.Instance.Items
                .Where(predicate =>
                {
                    return predicate.Id == id;
                })
                .FirstOrDefault();

            items
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

            for (int i = 0; i < keyValuePairs.Count; i++)
            {
                var item = keyValuePairs[i];
                Add(item["id"].AsInt, item["count"].AsInt);
            }
        }
    }

    public void OnSavePrefs()
    {
        PlayerPrefs.SetString(prefKey, JsonConvert.SerializeObject(items));
        PlayerPrefs.Save();
    }
}
