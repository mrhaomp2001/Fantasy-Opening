using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Farm/Items/ItemArmorLeg")]
public class ItemArmorLeg : ItemBase
{
    [SerializeField] private GameStatCollection stats;

    public GameStatCollection Stats { get => stats; set => stats = value; }
}
