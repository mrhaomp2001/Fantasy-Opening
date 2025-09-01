using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Farm/Items/Weapon")]
public class ItemWeapon : ItemBase
{
    [SerializeField] private string projectile;
    [SerializeField] private GameStatCollection stats;
    public override string ItemDescription { get => base.ItemDescription + "\n" + stats.GetString(); set => base.ItemDescription = value; }
    public string Projectile { get => projectile; set => projectile = value; }
    public GameStatCollection Stats { get => stats; set => stats = value; }
}
