using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[JsonObject(MemberSerialization.OptIn), System.Serializable]

public class GameStatCollection
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

    public string GetString()
    {
        string result = "";

        if (hpMax != 0) result += $"HP: {hpMax}\n";
        if (hpRegeneration != 0) result += $"HP Regen: {hpRegeneration}\n";
        if (damageGlobalBonus != 0) result += $"DMG: {damageGlobalBonus}\n";
        if (meleeDamageBonus != 0) result += $"Melee DMG: {meleeDamageBonus}\n";
        if (rangeDamageBonus != 0) result += $"Range DMG: {rangeDamageBonus}\n";
        if (magicDamageBonus != 0) result += $"Magic DMG: {magicDamageBonus}\n";
        if (attackSpeedBonus != 0) result += $"Attack SPD: {attackSpeedBonus}\n";
        if (critChance != 0) result += $"critChance: {critChance}\n";
        if (range != 0) result += $"Range: {range}\n";
        if (dodge != 0) result += $"Dodge: {dodge}\n";
        if (speed != 0) result += $"SPD: {speed}\n";
        if (curse != 0) result += $"Curse: {curse}\n";

        return result.TrimEnd();
    }

    public static GameStatCollection Add(GameStatCollection a, GameStatCollection b)
    {
        return new GameStatCollection
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
            curse = a.curse + b.curse
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
        UpdateHp();
        UpdateExp();
    }

    public void UpdateHp()
    {
        sliderHp.maxValue = InventoryController.Instance.GetPlayerData.Stats.HpMax;
        sliderHp.value = InventoryController.Instance.GetPlayerData.Hp;

        textHp.text = $"{sliderHp.value}/{sliderHp.maxValue}";
    }

    public void UpdateExp()
    {
        if (InventoryController.Instance.GetPlayerData.ExpNeededCurrent != null)
        {
            sliderExp.maxValue = InventoryController.Instance.GetPlayerData.ExpNeededCurrent.ExpNeeded;
        }

        sliderExp.value = InventoryController.Instance.GetPlayerData.Exp;

        textExHp.text = $"{InventoryController.Instance.GetPlayerData.Level}";
    }
}
