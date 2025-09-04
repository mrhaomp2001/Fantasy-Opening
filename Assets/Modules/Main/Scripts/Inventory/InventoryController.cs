using Newtonsoft.Json;
using SimpleJSON;
using System;
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
        [SerializeField] private int hp;
        [JsonProperty]
        [SerializeField] private int hotbarSelectedSlot;

        [JsonProperty]
        [SerializeField] private int level;
        [JsonProperty]
        [SerializeField] private int exp;

        [JsonProperty]
        [SerializeField] private List<InventoryItem> items = new();

        [JsonProperty]
        [SerializeField] private List<InventoryItem> hotbar = new();

        [JsonProperty]
        [SerializeField] private InventoryItem armorHead = new();
        [JsonProperty]
        [SerializeField] private InventoryItem armorBody = new();
        [JsonProperty]
        [SerializeField] private InventoryItem armorLeg = new();
        [JsonProperty]
        [SerializeField] private InventoryItem armorFoot = new();

        [JsonProperty]
        [SerializeField] private List<ProgressionBase> progressions;
        [JsonProperty]
        [SerializeField] private List<BuffBase> buffs;

        [JsonProperty]
        [SerializeField] private BuildingController.BuildingData buildingData;
        [JsonProperty]
        [SerializeField] private StatCollection stats;
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


        public ExpPerLevel ExpNeededCurrent
        {
            get
            {
                var expNeeded = ItemDatabase.Instance.ExpPerLevels
                    .Where(predicate =>
                    {
                        return predicate.Level == level;
                    })
                    .FirstOrDefault();

                return expNeeded;
            }
        }

        public StatCollection StatCollectionFinal
        {
            get
            {
                StatCollection result = stats
                     + (armorHead.item as ItemArmorHead)?.Stats
                     + (armorBody.item as ItemArmorBody)?.Stats
                     + (armorLeg.item as ItemArmorLeg)?.Stats
                     + (armorFoot.item as ItemArmorFoot)?.Stats;

                if (Buffs != null)
                {
                    for (int i = 0; i < Buffs.Count; i++)
                    {
                        var buff = Buffs[i];
                        if (buff != null && buff.Stats != null)
                        {
                            result += buff.Stats;
                        }
                    }
                }

                if (SelectedHotbar.item != null && SelectedHotbar.item is ItemWeapon weapon)
                {
                    result += weapon.Stats;
                }
                return result;
            }
        }

        public int Hp { get => hp/*Mathf.Clamp(hp, 0, hpMax)*/; set => hp = value; }

        public int HpMax
        {
            get
            {
                return StatCollectionFinal.HpMax;
            }
        }

        public int Attack
        {
            get
            {
                int result = 0;

                result += StatCollectionFinal.DamageGlobalBonus;
                
                if (SelectedHotbar.item is ItemWeapon weapon)
                {

                    if (weapon.WeaponType==WeaponType.Melee)
                    {
                        result += StatCollectionFinal.MeleeDamageBonus;
                    }

                    if (weapon.WeaponType==WeaponType.Range)
                    {
                        result += StatCollectionFinal.RangeDamageBonus;
                    }

                    if (weapon.WeaponType==WeaponType.Magic)
                    {
                        result += StatCollectionFinal.MagicDamageBonus;
                    }
                }

                return result;
            }
        }

        public int AttackSpeed
        {
            get
            {
                return StatCollectionFinal.AttackSpeedBonus;
            }
        }

        public int AttackRange
        {
            get
            {
                return StatCollectionFinal.Range;
            }
        }

        public int Defend
        {
            get
            {
                return StatCollectionFinal.Defend;
            }
        }

        public int Speed
        {
            get
            {
                return StatCollectionFinal.Speed;
            }
        }

        public InventoryItem SelectedHotbar
        {
            get
            {
                if (Hotbar.Count > 0)
                {
                    return Hotbar[HotbarSelectedSlot];
                }
                return null;
            }
        }
        public BuildingController.BuildingData BuildingData { get => buildingData; set => buildingData = value; }
        public InventoryItem ArmorHead { get => armorHead; set => armorHead = value; }
        public InventoryItem ArmorBody { get => armorBody; set => armorBody = value; }
        public InventoryItem ArmorLeg { get => armorLeg; set => armorLeg = value; }
        public InventoryItem ArmorFoot { get => armorFoot; set => armorFoot = value; }
        public List<ProgressionBase> Progressions { get => progressions; set => progressions = value; }
        public StatCollection Stats { get => stats; set => stats = value; }
        public int Level { get => level; set => level = value; }
        public int Exp { get => exp; set => exp = value; }
        public List<BuffBase> ReversedBuffs
        {
            get
            {
                if (Buffs == null) return null;

                var reversed = new List<BuffBase>(Buffs.Count);
                for (int i = Buffs.Count - 1; i >= 0; i--)
                {
                    reversed.Add(Buffs[i]);
                }

                return reversed;
            }
            set => Buffs = value; 
        }

        public List<BuffBase> Buffs { get => buffs; set => buffs = value; }
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
        var targetItem = ItemDatabase.Instance.Items
            .Where(predicate =>
            {
                return predicate.Id == id;
            })
            .FirstOrDefault();

        if (!targetItem.IsNonStack)
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
        }

        if (playerData.IsInventoryFull)
        {
            Debug.Log("Inventory is full, cannot add item with id: " + id);
            PopUpInventory.Instance.UpdateViewHotbar();

            return false;
        }



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
        playerData = new PlayerData();

        playerData.Stats = new StatCollection();

        playerData.Stats.HpMax = 100;
        playerData.Stats.HpRegeneration = 0;
        playerData.Stats.DamageGlobalBonus = 1;
        playerData.Stats.MeleeDamageBonus = 0;
        playerData.Stats.RangeDamageBonus = 0;
        playerData.Stats.MagicDamageBonus = 0;
        playerData.Stats.AttackSpeedBonus = 0;
        playerData.Stats.CritChance = 20;
        playerData.Stats.Range = 1;
        playerData.Stats.Dodge = 20;
        playerData.Stats.Speed = 5;
        playerData.Stats.Curse = 0;
        playerData.Stats.Defend = 0;

        playerData.Level = 1;
        playerData.Exp = 0;

        playerData.Hp = 100;

        playerData.Hotbar = new();
        playerData.Items = new();

        playerData.ArmorHead = new InventoryItem();
        playerData.ArmorBody = new InventoryItem();
        playerData.ArmorLeg = new InventoryItem();
        playerData.ArmorFoot = new InventoryItem();

        playerData.Buffs = new();



        for (int i = 0; i < 27; i++)
        {
            playerData.Items.Add(new());// i
        }

        for (int i = 0; i < 9; i++)
        {
            playerData.Hotbar.Add(new());// i
        }

        if (playerData.BuildingData != null)
        {
            if (playerData.BuildingData.Buildings != null)
            {
                foreach (var item in playerData.BuildingData.Buildings)
                {
                    if (item != null)
                    {
                        BuildingController.Instance.DestroyBuilding(item.Id);
                    }
                }
            }

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

            LoadEquipment(keyValuePairs);

            BuildingController.Instance.Load(keyValuePairs["buildingData"]);

            ProgressionController.Instance.LoadData(keyValuePairs["progressions"]);

            playerData.Stats.HpMax = keyValuePairs["stats"]["hpMax"].AsInt;
            playerData.Stats.HpRegeneration = keyValuePairs["stats"]["hpRegeneration"].AsInt;
            playerData.Stats.DamageGlobalBonus = keyValuePairs["stats"]["damageGlobalBonus"].AsInt;
            playerData.Stats.MeleeDamageBonus = keyValuePairs["stats"]["meleeDamageBonus"].AsInt;
            playerData.Stats.RangeDamageBonus = keyValuePairs["stats"]["rangeDamageBonus"].AsInt;
            playerData.Stats.MagicDamageBonus = keyValuePairs["stats"]["magicGlobalBonus"].AsInt;
            playerData.Stats.AttackSpeedBonus = keyValuePairs["stats"]["attackSpeedBonus"].AsInt;
            playerData.Stats.CritChance = keyValuePairs["plastatsyerStats"]["critChance"].AsInt;
            playerData.Stats.Range = keyValuePairs["stats"]["range"].AsInt;
            playerData.Stats.Dodge = keyValuePairs["stats"]["dodge"].AsInt;
            playerData.Stats.Speed = keyValuePairs["stats"]["speed"].AsInt;
            playerData.Stats.Curse = keyValuePairs["stats"]["curse"].AsInt;

            playerData.Hp = keyValuePairs["hp"].AsInt;
            playerData.Exp = keyValuePairs["exp"].AsInt;
            playerData.Level = keyValuePairs["level"].AsInt;

            for (int i = 0; i < keyValuePairs["buffs"].Count; i++)
            {
                var buffItem = keyValuePairs["buffs"][i];
                var buff = ItemDatabase.Instance.Buffs
                    .Where(predicate =>
                    {
                        return predicate.Id == buffItem["id"].AsInt;
                    })
                    .FirstOrDefault();

                if (buff != null)
                {
                    playerData.Buffs.Add(buff);
                }
            }
        }

        PopUpInventory.Instance.UpdateViewHotbar();

        StatController.Instance.UpdateViews();

        ProgressionController.Instance.Load();

        Save();
    }

    public void AddExp(int value)
    {
        playerData.Exp += value;

        UpdateExp();
        StatController.Instance.UpdateExp();
    }

    private void UpdateExp()
    {
        if (playerData.ExpNeededCurrent != null)
        {
            if (playerData.Exp >= playerData.ExpNeededCurrent.ExpNeeded)
            {
                playerData.Exp -= playerData.ExpNeededCurrent.ExpNeeded;
                playerData.Level++;
                UpdateExp();

                GameController.Instance.OnLevelUp();
            }
        }
    }

    private void LoadEquipment(JSONNode keyValuePairs)
    {
        if (keyValuePairs["armorHead"] != null && keyValuePairs["armorHead"]["item"] != null && keyValuePairs["armorHead"]["item"]["id"] != null && keyValuePairs["armorHead"]["item"]["id"] != "")
        {
            var jsonValue = keyValuePairs["armorHead"]["item"];

            var item = ItemDatabase.Instance.Items
                .Where(predicate =>
                {
                    return predicate.Id == jsonValue["id"].AsInt;
                })
                .FirstOrDefault();

            Add(item.Id, 1);

            EquipEquipment(item);
        }

        if (keyValuePairs["armorBody"] != null && keyValuePairs["armorBody"]["item"] != null && keyValuePairs["armorBody"]["item"]["id"] != null && keyValuePairs["armorBody"]["item"]["id"] != "")
        {
            var jsonValue = keyValuePairs["armorBody"]["item"];

            var item = ItemDatabase.Instance.Items
                .Where(predicate =>
                {
                    return predicate.Id == jsonValue["id"].AsInt;
                })
                .FirstOrDefault();

            Add(item.Id, 1);

            EquipEquipment(item);
        }

        if (keyValuePairs["armorLeg"] != null && keyValuePairs["armorLeg"]["item"] != null && keyValuePairs["armorLeg"]["item"]["id"] != null && keyValuePairs["armorLeg"]["item"]["id"] != "")
        {
            var jsonValue = keyValuePairs["armorLeg"]["item"];

            var item = ItemDatabase.Instance.Items
                .Where(predicate =>
                {
                    return predicate.Id == jsonValue["id"].AsInt;
                })
                .FirstOrDefault();

            Add(item.Id, 1);

            EquipEquipment(item);
        }

        if (keyValuePairs["armorFoot"] != null && keyValuePairs["armorFoot"]["item"] != null && keyValuePairs["armorFoot"]["item"]["id"] != null && keyValuePairs["armorFoot"]["item"]["id"] != "")
        {
            var jsonValue = keyValuePairs["armorFoot"]["item"];

            var item = ItemDatabase.Instance.Items
                .Where(predicate =>
                {
                    return predicate.Id == jsonValue["id"].AsInt;
                })
                .FirstOrDefault();

            Add(item.Id, 1);

            EquipEquipment(item);
        }
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

    public void EquipEquipment(ItemBase itemTarget)
    {
        if (itemTarget != null)
        {
            var itemHolder = ItemDatabase.Instance.Items
                .Where(predicate =>
                {
                    return predicate != null && predicate.Id == itemTarget.Id;
                })
                .FirstOrDefault();


            if (itemTarget is ItemArmorHead head)
            {
                int currentArmorId = 0;
                if (playerData.ArmorHead.item != null)
                {
                    currentArmorId = playerData.ArmorHead.item.Id;
                }

                Consume(itemTarget.Id, 1, new Callback
                {
                    onSuccess = () =>
                    {
                        playerData.ArmorHead.item = itemHolder;
                        playerData.ArmorHead.count = 1;
                    },
                    onFail = (message) =>
                    {
                    },
                    onNext = () =>
                    {
                        if (currentArmorId != 0)
                        {
                            Add(currentArmorId, 1, isNonCheckHotbar: true);
                        }
                    }
                });
            }

            if (itemTarget is ItemArmorBody body)
            {
                int currentArmorId = 0;
                if (playerData.ArmorBody.item != null)
                {
                    currentArmorId = playerData.ArmorBody.item.Id;
                }

                Consume(itemTarget.Id, 1, new Callback
                {
                    onSuccess = () =>
                    {
                        playerData.ArmorBody.item = itemHolder;
                        playerData.ArmorBody.count = 1;
                    },
                    onFail = (message) =>
                    {
                    },
                    onNext = () =>
                    {
                        if (currentArmorId != 0)
                        {
                            Add(currentArmorId, 1, isNonCheckHotbar: true);
                        }
                    }
                });
            }

            if (itemTarget is ItemArmorLeg leg)
            {
                int currentArmorId = 0;
                if (playerData.ArmorLeg.item != null)
                {
                    currentArmorId = playerData.ArmorLeg.item.Id;
                }

                Consume(itemTarget.Id, 1, new Callback
                {
                    onSuccess = () =>
                    {
                        playerData.ArmorLeg.item = itemHolder;
                        playerData.ArmorLeg.count = 1;
                    },
                    onFail = (message) =>
                    {
                    },
                    onNext = () =>
                    {
                        if (currentArmorId != 0)
                        {
                            Add(currentArmorId, 1, isNonCheckHotbar: true);
                        }
                    }
                });
            }

            if (itemTarget is ItemArmorFoot foot)
            {
                int currentArmorId = 0;
                if (playerData.ArmorFoot.item != null)
                {
                    currentArmorId = playerData.ArmorFoot.item.Id;
                }

                Consume(itemTarget.Id, 1, new Callback
                {
                    onSuccess = () =>
                    {
                        playerData.ArmorFoot.item = itemHolder;
                        playerData.ArmorFoot.count = 1;
                    },
                    onFail = (message) =>
                    {
                    },
                    onNext = () =>
                    {
                        if (currentArmorId != 0)
                        {
                            Add(currentArmorId, 1, isNonCheckHotbar: true);
                        }
                    }
                });
            }


            PopUpInventory.Instance.UpdateViews();
            PopUpInventory.Instance.UpdateViewHotbar();
        }
    }

    public void Save()
    {
        BuildingController.Instance.Save();
        ProgressionController.Instance.Save();

        Debug.Log($"OnSavePrefs: {JsonConvert.SerializeObject(playerData)}");
        PlayerPrefs.SetString(prefKey, JsonConvert.SerializeObject(playerData));
        PlayerPrefs.Save();
    }
}
