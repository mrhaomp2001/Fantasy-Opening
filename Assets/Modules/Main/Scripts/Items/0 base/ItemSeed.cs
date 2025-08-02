using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Farm/Items/Seed")]

public class ItemSeed : ItemBase
{
    [SerializeField] private int cropId;

    public int CropId { get => cropId; set => cropId = value; }
}
