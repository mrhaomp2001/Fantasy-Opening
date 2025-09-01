using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Farm/Items/ItemArmorBody")]
public class ItemArmorBody : ItemBase
{
    [SerializeField] private GameStatCollection stats;

    public GameStatCollection Stats { get => stats; set => stats = value; }
}
