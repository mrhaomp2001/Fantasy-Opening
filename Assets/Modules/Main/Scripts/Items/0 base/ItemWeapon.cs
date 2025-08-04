using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Farm/Items/Weapon")]
public class ItemWeapon : ItemBase
{
    [SerializeField] private string projectile;

    public string Projectile { get => projectile; set => projectile = value; }
}
