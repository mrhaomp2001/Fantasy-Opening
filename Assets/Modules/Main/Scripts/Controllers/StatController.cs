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
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        void AppendIfPositive(string name, int value)
        {
            if (value != 0)
                sb.AppendLine($"{name}+ {value}");
        }

        AppendIfPositive(nameof(hpMax), hpMax);
        AppendIfPositive(nameof(hpRegeneration), hpRegeneration);
        AppendIfPositive(nameof(damageGlobalBonus), damageGlobalBonus);
        AppendIfPositive(nameof(meleeDamageBonus), meleeDamageBonus);
        AppendIfPositive(nameof(rangeDamageBonus), rangeDamageBonus);
        AppendIfPositive(nameof(magicDamageBonus), magicDamageBonus);
        AppendIfPositive(nameof(attackSpeedBonus), attackSpeedBonus);
        AppendIfPositive(nameof(critChance), critChance);
        AppendIfPositive(nameof(range), range);
        AppendIfPositive(nameof(dodge), dodge);
        AppendIfPositive(nameof(speed), speed);
        AppendIfPositive(nameof(curse), curse);

        return sb.ToString().TrimEnd();
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
        sliderHp.maxValue = InventoryController.Instance.GetPlayerData.PlayerStats.HpMax;
        sliderHp.value = InventoryController.Instance.GetPlayerData.Hp;

        textHp.text = $"{sliderHp.value}/{sliderHp.maxValue}";
    }

    public void UpdateExp()
    {
        sliderExp.maxValue = InventoryController.Instance.GetPlayerData.ExpNeededCurrent.ExpNeeded;
        sliderExp.value = InventoryController.Instance.GetPlayerData.Exp;

        textExHp.text = $"{InventoryController.Instance.GetPlayerData.Level}";
    }
}
