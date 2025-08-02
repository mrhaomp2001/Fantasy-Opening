using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemBase : ScriptableObject
{
    public enum ItemType
    {
        Crop,
        Seed,
        Material,
    }

    [SerializeField] private int id;
    [SerializeField] private ItemType type;
    [SerializeField] private string itemName;
    [SerializeField] private Sprite sprite;
    [SerializeField] private int buyPrice;
    [SerializeField] private int sellPrice;

    public int Id { get => id; set => id = value; }
    public string ItemName { get => itemName; set => itemName = value; }
    public Sprite Sprite { get => sprite; set => sprite = value; }
    public ItemType Type { get => type; set => type = value; }

    /// <summary>
    /// It's Mean Player will Sell this with this Price
    /// </summary>
    public int SellPrice { get => sellPrice; set => sellPrice = value; }
    
    /// <summary>
    /// It's Mean Player will Buy this with this Price
    /// </summary>
    public int BuyPrice { get => buyPrice; set => buyPrice = value; }
}
