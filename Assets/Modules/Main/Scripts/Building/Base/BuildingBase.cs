using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBase : MonoBehaviour, IWorldInteractable
{
    [SerializeField] protected int id;
    [SerializeField] private int idItem;
    [SerializeField] private int hp;
    [SerializeField] private int hpMax;
    [SerializeField] private int def;

    public int Id { get => id; set => id = value; }
    public int IdItem { get => idItem; set => idItem = value; }

    public void Initialize()
    {
        hp = hpMax;
    }

    public void NextDay()
    {
        hp = hpMax;
    }

    public void OnDestroyBuilding()
    {
        WorldItemController.Instance.SpawnItem(idItem, transform.position);
    }

    public void OnTakeDamage(PlayerBullet bullet)
    {
        int dmg = Mathf.Max(0, bullet.Damage - def);
        if (dmg > 0)
        {
            hp -= dmg;
            if (hp <= 0)
            {
                BuildingController.Instance.DestroyBuilding(id);

            }

        }
    }

    public virtual void OnWorldInteract()
    {

    }
}
