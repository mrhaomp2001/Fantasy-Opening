using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[JsonObject(MemberSerialization.OptIn), System.Serializable]
public class ItemBase : ScriptableObject
{
    [JsonProperty]
    [SerializeField] private int id;

    [SerializeField] private string itemName;
    [SerializeField, TextArea(3, 10)] private string itemDescription;
    [SerializeField] private Sprite sprite;
    [SerializeField] private Sprite spriteWorldItem;
    [SerializeField] private int buyPrice;
    [SerializeField] private int sellPrice;
    [SerializeField] private bool isNonStack;

    public int Id { get => id; set => id = value; }
    public string ItemName { get => LanguageController.Instance.GetString(itemName); set => itemName = value; }
    public string ItemDescription { get => LanguageController.Instance.GetString(itemDescription); set => itemDescription = value; }
    public Sprite Sprite { get => sprite; set => sprite = value; }

    /// <summary>
    /// It's Mean Player will Sell this with this Price
    /// </summary>
    public int SellPrice { get => sellPrice; set => sellPrice = value; }

    /// <summary>
    /// It's Mean Player will Buy this with this Price
    /// </summary>
    public int BuyPrice { get => buyPrice; set => buyPrice = value; }
    public Sprite SpriteWorldItem { get => spriteWorldItem; set => spriteWorldItem = value; }
    public bool IsNonStack { get => isNonStack; set => isNonStack = value; }
}
