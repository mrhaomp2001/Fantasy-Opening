using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Farm/Items/ItemArmorFoot")]
public class ItemArmorFoot : ItemBase
{
    [SerializeField] private StatCollection stats;

    public StatCollection Stats { get => stats; set => stats = value; }

    public override string ItemDescription { get => base.ItemDescription + "\n" + stats.GetString(); set => base.ItemDescription = value; }

}
