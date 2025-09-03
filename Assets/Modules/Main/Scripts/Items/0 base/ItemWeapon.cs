using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Melee = 0,
    Range = 1,
    Magic = 2,
}

[CreateAssetMenu(fileName = "New Item", menuName = "Farm/Items/Weapon")]
public class ItemWeapon : ItemBase
{
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private string projectile;
    [SerializeField] private StatCollection stats;
    public override string ItemDescription { get => base.ItemDescription + "\n" + stats.GetString(); set => base.ItemDescription = value; }
    public string Projectile { get => projectile; set => projectile = value; }
    public StatCollection Stats { get => stats; set => stats = value; }
    public WeaponType WeaponType { get => weaponType; set => weaponType = value; }
}
