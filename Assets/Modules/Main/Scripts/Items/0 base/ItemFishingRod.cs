using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FishingRodStatCollection
{
    [SerializeField] private float minFishingTime;
    [SerializeField] private float maxFishingTime;

    [Header("--")]
    [SerializeField] private float superRareFishRate;
    [SerializeField] private List<ItemBase> superRareFishs;
    [Header("--")]
    [SerializeField] private float rareFishRate;
    [SerializeField] private List<ItemBase> rareFishs;
    [Header("--")]
    [SerializeField] private float commonFishRate;
    [SerializeField] private List<ItemBase> commonFishs;

    public FishingRodStatCollection()
    {
        minFishingTime = 0f;
        maxFishingTime = 0f;
        superRareFishRate = 0f;
        superRareFishs = new List<ItemBase>();
        rareFishRate = 0f;
        rareFishs = new List<ItemBase>();
        commonFishRate = 0f;
        commonFishs = new List<ItemBase>();

    }

    public FishingRodStatCollection(float minFishingTime, float maxFishingTime, float superRareFishRate, List<ItemBase> superRareFishs, float rareFishRate, List<ItemBase> rareFishs, float commonFishRate, List<ItemBase> commonFishs)
    {
        this.minFishingTime = minFishingTime;
        this.maxFishingTime = maxFishingTime;
        this.superRareFishRate = superRareFishRate;
        this.superRareFishs = superRareFishs;
        this.rareFishRate = rareFishRate;
        this.rareFishs = rareFishs;
        this.commonFishRate = commonFishRate;
        this.commonFishs = commonFishs;
    }

    public float MinFishingTime { get => minFishingTime; set => minFishingTime = value; }
    public float MaxFishingTime { get => maxFishingTime; set => maxFishingTime = value; }
    public float SuperRareFishRate { get => superRareFishRate; set => superRareFishRate = value; }
    public List<ItemBase> SuperRareFishs { get => superRareFishs; set => superRareFishs = value; }
    public float RareFishRate { get => rareFishRate; set => rareFishRate = value; }
    public List<ItemBase> RareFishs { get => rareFishs; set => rareFishs = value; }
    public float CommonFishRate { get => commonFishRate; set => commonFishRate = value; }
    public List<ItemBase> CommonFishs { get => commonFishs; set => commonFishs = value; }
}

[CreateAssetMenu(fileName = "New Item", menuName = "Farm/Items/Fishing Rod")]

public class ItemFishingRod : ItemInteractable
{
    [Header("Stats: ")]

    [SerializeField] private FishingRodStatCollection stats;
}
