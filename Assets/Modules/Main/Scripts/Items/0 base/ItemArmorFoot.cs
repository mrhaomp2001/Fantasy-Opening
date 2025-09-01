using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Farm/Items/ItemArmorFoot")]
public class ItemArmorFoot : ItemBase
{
    [SerializeField] private GameStatCollection stats;

    public GameStatCollection Stats { get => stats; set => stats = value; }
}
