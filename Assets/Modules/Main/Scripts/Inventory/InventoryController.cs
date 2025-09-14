using Newtonsoft.Json;
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public static class StringExtensions
{
    public static string ToCamel(this string s)
    {
        if (string.IsNullOrEmpty(s)) return s;
        if (s.Length == 1) return s.ToLowerInvariant();
        return char.ToLowerInvariant(s[0]) + s.Substring(1);
    }
}


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
        [SerializeField] private int hunger;
        [JsonProperty]
        [SerializeField] private int hungerMax;

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
        [JsonProperty]
        [SerializeField] private List<RecipeWithCondition> recipes;


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

                    if (weapon.WeaponType == WeaponType.Melee)
                    {
                        result += StatCollectionFinal.MeleeDamageBonus;
                    }

                    if (weapon.WeaponType == WeaponType.Range)
                    {
                        result += StatCollectionFinal.RangeDamageBonus;
                    }

                    if (weapon.WeaponType == WeaponType.Magic)
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
        public List<RecipeWithCondition> Recipes { get => recipes; set => recipes = value; }
        public int Hunger { get => hunger; set { hunger = value; } }
        public int HungerMax { get => hungerMax; set => hungerMax = value; }
    }

    private static InventoryController instance;
    private const string prefKey = "InventoryController";

    [SerializeField] private int money;

    [SerializeField] private List<RecipeWithCondition> recipes;


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

        playerData.Stats = new StatCollection
        {
            HpMax = 100,
            HpRegeneration = 0,
            DamageGlobalBonus = 1,
            MeleeDamageBonus = 0,
            RangeDamageBonus = 0,
            MagicDamageBonus = 0,
            AttackSpeedBonus = 0,
            CritChance = 20,
            Range = 1,
            Dodge = 20,
            Speed = 5,
            Curse = 0,
            Defend = 0
        };

        playerData.Level = 1;
        playerData.Exp = 0;
        playerData.Hp = 100;


        playerData.HungerMax = 100;
        playerData.Hunger = 100;

        playerData.Hotbar = new();
        playerData.Items = new();

        playerData.ArmorHead = new InventoryItem();
        playerData.ArmorBody = new InventoryItem();
        playerData.ArmorLeg = new InventoryItem();
        playerData.ArmorFoot = new InventoryItem();

        playerData.Buffs = new();

        playerData.Recipes = recipes;

        for (int i = 0; i < 27; i++)
        {
            playerData.Items.Add(new());
        }

        for (int i = 0; i < 9; i++)
        {
            playerData.Hotbar.Add(new());
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

            var hotbarSelectedKey = nameof(playerData.HotbarSelectedSlot).ToCamel();
            var hotbarKey = nameof(playerData.Hotbar).ToCamel();
            var itemsKey = nameof(playerData.Items).ToCamel();
            var buildingDataKey = nameof(playerData.BuildingData).ToCamel();
            var progressionsKey = nameof(playerData.Progressions).ToCamel();
            var statsKey = nameof(playerData.Stats).ToCamel();
            var hpKey = nameof(playerData.Hp).ToCamel();

            var hungerKey = nameof(playerData.Hunger).ToCamel();
            var hungerMaxKey = nameof(playerData.HungerMax).ToCamel();

            var expKey = nameof(playerData.Exp).ToCamel();
            var levelKey = nameof(playerData.Level).ToCamel();
            var buffsKey = nameof(playerData.Buffs).ToCamel();

            // hotbar selected
            if (keyValuePairs[hotbarSelectedKey] != null)
                playerData.HotbarSelectedSlot = keyValuePairs[hotbarSelectedKey].AsInt;

            // hotbar
            var hotbarJson = keyValuePairs[hotbarKey];
            if (hotbarJson != null)
            {
                for (int i = 0; i < hotbarJson.Count; i++)
                {
                    var item = hotbarJson[i];
                    if (item != null && item[nameof(InventoryItem.item)] != null)
                    {
                        var itemJson = item[nameof(InventoryItem.item)];
                        var itemIdKey = nameof(InventoryItem.item.Id).ToCamel();
                        if (itemJson[itemIdKey].AsInt != 0)
                        {
                            Add(itemJson[itemIdKey].AsInt, item[nameof(InventoryItem.count)].AsInt);

                            SelectHotbarSlot(i, playerData.Items
                                .Where(predicate => predicate.item != null && predicate.item.Id == itemJson[itemIdKey].AsInt)
                                .FirstOrDefault());
                        }
                    }
                }
            }

            // items
            var itemsJson = keyValuePairs[itemsKey];
            if (itemsJson != null)
            {
                var itemIdKey = nameof(InventoryItem.item.Id).ToCamel();
                var itemCountKey = nameof(InventoryItem.count).ToCamel();
                for (int i = 0; i < itemsJson.Count; i++)
                {
                    var item = itemsJson[i];
                    if (item[nameof(InventoryItem.item)] != null)
                    {
                        Add(item[nameof(InventoryItem.item)][itemIdKey].AsInt,
                            item[itemCountKey].AsInt);
                    }
                }
            }

            // equipment
            LoadEquipment(keyValuePairs);
            LoadRecipe(keyValuePairs);

            // building & progression
            BuildingController.Instance.Load(keyValuePairs[buildingDataKey]);
            ProgressionController.Instance.Load(keyValuePairs[progressionsKey]);

            // stats (use camelCase keys inside stats object)
            if (keyValuePairs[statsKey] != null)
            {
                var s = keyValuePairs[statsKey];
                playerData.Stats.HpMax = s[nameof(playerData.Stats.HpMax).ToCamel()].AsInt;
                playerData.Stats.HpRegeneration = s[nameof(playerData.Stats.HpRegeneration).ToCamel()].AsInt;
                playerData.Stats.DamageGlobalBonus = s[nameof(playerData.Stats.DamageGlobalBonus).ToCamel()].AsInt;
                playerData.Stats.MeleeDamageBonus = s[nameof(playerData.Stats.MeleeDamageBonus).ToCamel()].AsInt;
                playerData.Stats.RangeDamageBonus = s[nameof(playerData.Stats.RangeDamageBonus).ToCamel()].AsInt;
                playerData.Stats.MagicDamageBonus = s[nameof(playerData.Stats.MagicDamageBonus).ToCamel()].AsInt;
                playerData.Stats.AttackSpeedBonus = s[nameof(playerData.Stats.AttackSpeedBonus).ToCamel()].AsInt;
                playerData.Stats.CritChance = s[nameof(playerData.Stats.CritChance).ToCamel()].AsInt;
                playerData.Stats.Range = s[nameof(playerData.Stats.Range).ToCamel()].AsInt;
                playerData.Stats.Dodge = s[nameof(playerData.Stats.Dodge).ToCamel()].AsInt;
                playerData.Stats.Speed = s[nameof(playerData.Stats.Speed).ToCamel()].AsInt;
                playerData.Stats.Curse = s[nameof(playerData.Stats.Curse).ToCamel()].AsInt;
                playerData.Stats.Defend = s[nameof(playerData.Stats.Defend).ToCamel()].AsInt;
            }

            // hp, exp, level
            if (keyValuePairs[hpKey] != null) playerData.Hp = keyValuePairs[hpKey].AsInt;
            if (keyValuePairs[expKey] != null) playerData.Exp = keyValuePairs[expKey].AsInt;
            if (keyValuePairs[levelKey] != null) playerData.Level = keyValuePairs[levelKey].AsInt;

            if (keyValuePairs[hungerKey] != null) playerData.Hunger = keyValuePairs[hungerKey].AsInt;
            if (keyValuePairs[hungerMaxKey] != null) playerData.HungerMax = keyValuePairs[hungerMaxKey].AsInt;


            // buffs
            var buffsJson = keyValuePairs[buffsKey];
            if (buffsJson != null)
            {
                var buffIdKey = nameof(BuffBase.Id).ToCamel();
                for (int i = 0; i < buffsJson.Count; i++)
                {
                    var buffItem = buffsJson[i];
                    var buff = ItemDatabase.Instance.Buffs
                        .Where(predicate => predicate.Id == buffItem[buffIdKey].AsInt)
                        .FirstOrDefault();

                    if (buff != null)
                    {
                        playerData.Buffs.Add(buff);
                    }
                }
            }
        }

        PopUpInventory.Instance.UpdateViewHotbar();
        StatController.Instance.UpdateViews();
        ProgressionController.Instance.Load();
        Save();
    }

    private void LoadRecipe(JSONNode keyValuePairs)
    {
        var recipesJson = keyValuePairs[nameof(playerData.Recipes).ToCamel()];
        if (recipesJson != null)
        {
            var recipeIdKey = nameof(RecipeWithCondition.Id).ToCamel();
            var recipeIsUnlockedKey = nameof(RecipeWithCondition.IsUnlocked).ToCamel();
            for (int i = 0; i < recipesJson.Count; i++)
            {
                var recipeItem = recipesJson[i];
                var recipe = playerData.Recipes
                    .Where(predicate => predicate.Id == recipeItem[recipeIdKey].AsInt)
                    .FirstOrDefault();
                if (recipe != null)
                {
                    recipe.IsUnlocked = recipeItem[recipeIsUnlockedKey].AsBool;
                }
            }
        }
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
    
    public void ConsumeHunger(int value)
    {
        playerData.Hunger -= value;

        if (playerData.Hunger < 0)
        {
            Debug.Log($"Hunger: {playerData.Hunger}");
            PlayerController.Instance.Hurt(-playerData.Hunger);

            playerData.Hunger = 0;
        }

        StatController.Instance.UpdateHunger();
    }

    public void AddHunger(int value)
    {
        playerData.Hunger += value;

        if (playerData.Hunger > playerData.HungerMax)
        {
            playerData.Hunger = playerData.HungerMax;
        }

        StatController.Instance.UpdateHunger();

    }

    private void LoadEquipment(JSONNode keyValuePairs)
    {
        void LoadArmor(string armorKey)
        {
            if (keyValuePairs[armorKey] != null &&
                keyValuePairs[armorKey][nameof(InventoryItem.item)] != null &&
                keyValuePairs[armorKey][nameof(InventoryItem.item)][nameof(InventoryItem.item.Id).ToCamel()] != null &&
                keyValuePairs[armorKey][nameof(InventoryItem.item)][nameof(InventoryItem.item.Id).ToCamel()] != "")
            {
                var jsonValue = keyValuePairs[armorKey][nameof(InventoryItem.item)];

                var item = ItemDatabase.Instance.Items
                    .Where(predicate => predicate.Id == jsonValue[nameof(InventoryItem.item.Id).ToCamel()].AsInt)
                    .FirstOrDefault();

                if (item != null)
                {
                    Add(item.Id, 1);
                    EquipEquipment(item);
                }
            }
        }

        LoadArmor(nameof(playerData.ArmorHead).ToCamel());
        LoadArmor(nameof(playerData.ArmorBody).ToCamel());
        LoadArmor(nameof(playerData.ArmorLeg).ToCamel());
        LoadArmor(nameof(playerData.ArmorFoot).ToCamel());
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
