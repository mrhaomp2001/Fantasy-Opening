using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[JsonObject(MemberSerialization.OptIn), System.Serializable]

public class StatCollection
{
    [JsonProperty]
    [SerializeField] private int hpMax;
    [JsonProperty]
    [SerializeField] private int hpRegeneration;
    [JsonProperty]
    [SerializeField] private int damageGlobalBonus;
    [JsonProperty]
    [SerializeField] private int meleeDamageBonus;
    [JsonProperty]
    [SerializeField] private int rangeDamageBonus;
    [JsonProperty]
    [SerializeField] private int magicDamageBonus;
    [JsonProperty]
    [SerializeField] private int attackSpeedBonus;
    [JsonProperty]
    [SerializeField] private int critChance;
    [JsonProperty]
    [SerializeField] private int range;
    [JsonProperty]
    [SerializeField] private int dodge;
    [JsonProperty]
    [SerializeField] private int speed;
    [JsonProperty]
    [SerializeField] private int curse;
    [JsonProperty]
    [SerializeField] private int defend;

    public int HpMax { get => hpMax; set => hpMax = value; }
    public int HpRegeneration { get => hpRegeneration; set => hpRegeneration = value; }
    public int DamageGlobalBonus { get => damageGlobalBonus; set => damageGlobalBonus = value; }
    public int MeleeDamageBonus { get => meleeDamageBonus; set => meleeDamageBonus = value; }
    public int RangeDamageBonus { get => rangeDamageBonus; set => rangeDamageBonus = value; }
    public int MagicDamageBonus { get => magicDamageBonus; set => magicDamageBonus = value; }
    public int AttackSpeedBonus { get => attackSpeedBonus; set => attackSpeedBonus = value; }
    public int CritChance { get => critChance; set => critChance = value; }
    public int Range { get => range; set => range = value; }
    public int Dodge { get => dodge; set => dodge = value; }
    public int Speed { get => speed; set => speed = value; }
    public int Curse { get => curse; set => curse = value; }
    public int Defend { get => defend; set => defend = value; }

    public string GetString()
    {
        string result = "";

        if (hpMax != 0)
            result += $"{LanguageController.Instance.GetString("0_hpMax")}: {hpMax}\n";
        if (hpRegeneration != 0)
            result += $"{LanguageController.Instance.GetString("1_hpRegeneration")}: {hpRegeneration}\n";
        if (damageGlobalBonus != 0)
            result += $"{LanguageController.Instance.GetString("2_damageGlobalBonus")}: {damageGlobalBonus}\n";
        if (meleeDamageBonus != 0)
            result += $"{LanguageController.Instance.GetString("3_meleeDamageBonus")}: {meleeDamageBonus}\n";
        if (rangeDamageBonus != 0)
            result += $"{LanguageController.Instance.GetString("4_rangeDamageBonus")}: {rangeDamageBonus}\n";
        if (magicDamageBonus != 0)
            result += $"{LanguageController.Instance.GetString("5_magicDamageBonus")}: {magicDamageBonus}\n";
        if (attackSpeedBonus != 0)
            result += $"{LanguageController.Instance.GetString("6_attackSpeedBonus")}: {attackSpeedBonus}\n";
        if (critChance != 0)
            result += $"{LanguageController.Instance.GetString("7_critChance")}: {critChance}\n";
        if (range != 0)
            result += $"{LanguageController.Instance.GetString("8_range")}: {range}\n";
        if (dodge != 0)
            result += $"{LanguageController.Instance.GetString("9_dodge")}: {dodge}\n";
        if (speed != 0)
            result += $"{LanguageController.Instance.GetString("10_speed")}: {speed}\n";
        if (curse != 0)
            result += $"{LanguageController.Instance.GetString("11_curse")}: {curse}\n";
        if (defend != 0)
            result += $"{LanguageController.Instance.GetString("12_defend")}: {defend}\n";

        return result.TrimEnd();
    }
    public string GetStringFullAll()
    {
        string result = "";
        var map = new Dictionary<string, int>
    {
        { "0_hpMax", hpMax },
        { "1_hpRegeneration", hpRegeneration },
        { "2_damageGlobalBonus", damageGlobalBonus },
        { "3_meleeDamageBonus", meleeDamageBonus },
        { "4_rangeDamageBonus", rangeDamageBonus },
        { "5_magicDamageBonus", magicDamageBonus },
        { "6_attackSpeedBonus", attackSpeedBonus },
        { "7_critChance", critChance },
        { "8_range", range },
        { "9_dodge", dodge },
        { "10_speed", speed },
        { "11_curse", curse },
        { "12_defend", defend }
    };

        foreach (var kv in map)
        {
            result += $"{LanguageController.Instance.GetString(kv.Key)}: {kv.Value}\n";
        }

        return result.TrimEnd();
    }



    public static StatCollection operator +(StatCollection a, StatCollection b)
    {
        if (a == null && b == null) return new StatCollection();
        if (a == null) return b;
        if (b == null) return a;

        return new StatCollection
        {
            hpMax = a.hpMax + b.hpMax,
            hpRegeneration = a.hpRegeneration + b.hpRegeneration,
            damageGlobalBonus = a.damageGlobalBonus + b.damageGlobalBonus,
            meleeDamageBonus = a.meleeDamageBonus + b.meleeDamageBonus,
            rangeDamageBonus = a.rangeDamageBonus + b.rangeDamageBonus,
            magicDamageBonus = a.magicDamageBonus + b.magicDamageBonus,
            attackSpeedBonus = a.attackSpeedBonus + b.attackSpeedBonus,
            critChance = a.critChance + b.critChance,
            range = a.range + b.range,
            dodge = a.dodge + b.dodge,
            speed = a.speed + b.speed,
            curse = a.curse + b.curse,
            defend = a.defend + b.defend,
        };
    }


    public static StatCollection Add(StatCollection a, StatCollection b)
    {
        return new StatCollection
        {
            hpMax = a.hpMax + b.hpMax,
            hpRegeneration = a.hpRegeneration + b.hpRegeneration,
            damageGlobalBonus = a.damageGlobalBonus + b.damageGlobalBonus,
            meleeDamageBonus = a.meleeDamageBonus + b.meleeDamageBonus,
            rangeDamageBonus = a.rangeDamageBonus + b.rangeDamageBonus,
            magicDamageBonus = a.magicDamageBonus + b.magicDamageBonus,
            attackSpeedBonus = a.attackSpeedBonus + b.attackSpeedBonus,
            critChance = a.critChance + b.critChance,
            range = a.range + b.range,
            dodge = a.dodge + b.dodge,
            speed = a.speed + b.speed,
            curse = a.curse + b.curse,
            defend = a.defend + b.defend,
        };
    }
}
public class StatController : MonoBehaviour
{
    private static StatController instance;
    [Header("HP: ")]
    [SerializeField] private Slider sliderHp;
    [SerializeField] private TextMeshProUGUI textHp;
    [Header("EXP: ")]
    [SerializeField] private Slider sliderExp;
    [SerializeField] private TextMeshProUGUI textExHp;

    public static StatController Instance { get => instance; set => instance = value; }

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
        sliderHp.minValue = 0;
    }

    public void UpdateViews()
    {
        UpdateStats();

        UpdateHp();
        UpdateExp();
    }

    public void UpdateHp()
    {
        sliderHp.maxValue = InventoryController.Instance.GetPlayerData.HpMax;
        sliderHp.value = InventoryController.Instance.GetPlayerData.Hp;

        textHp.SetText($"{sliderHp.value}/{sliderHp.maxValue}");
    }

    public void UpdateExp()
    {
        if (InventoryController.Instance.GetPlayerData.ExpNeededCurrent != null)
        {
            sliderExp.maxValue = InventoryController.Instance.GetPlayerData.ExpNeededCurrent.ExpNeeded;
        }

        sliderExp.value = InventoryController.Instance.GetPlayerData.Exp;

        textExHp.SetText($"{InventoryController.Instance.GetPlayerData.Level}");
    }

    public void UpdateStats()
    {
        PlayerController.Instance.Speed = InventoryController.Instance.GetPlayerData.Speed;
    }
}
