using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Farm/Items/ItemArmorHead")]
public class ItemArmorHead : ItemBase
{
    [SerializeField] private GameStatCollection stats;

    public GameStatCollection Stats { get => stats; set => stats = value; }
}
