using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FishingRodStatCollection
{
    [SerializeField] private float minFishingTime;
    [SerializeField] private float maxFishingTime;

    [Header("--")]
    [SerializeField] private int hungerCost;

    [Header("--")]
    [SerializeField] private int superRareFishRate;
    [Header("--")]
    [SerializeField] private int rareFishRate;
    [Header("--")]
    [SerializeField] private int commonFishRate;

    public float MinFishingTime { get => minFishingTime; set => minFishingTime = value; }
    public float MaxFishingTime { get => maxFishingTime; set => maxFishingTime = value; }
    public int SuperRareFishRate { get => superRareFishRate; set => superRareFishRate = value; }
    public int RareFishRate { get => rareFishRate; set => rareFishRate = value; }
    public int CommonFishRate { get => commonFishRate; set => commonFishRate = value; }
    public int HungerCost { get => hungerCost; set => hungerCost = value; }
}

[CreateAssetMenu(fileName = "New Item", menuName = "Farm/Items/Fishing Rod")]

public class ItemFishingRod : ItemInteractable
{
    [Header("Stats: ")]

    [SerializeField] private FishingRodStatCollection stats;

    public FishingRodStatCollection Stats { get => stats; set => stats = value; }
}
