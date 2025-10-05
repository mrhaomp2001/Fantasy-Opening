using GameUtil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour, IPoolObject
{
    [SerializeField] protected float speed;
    [SerializeField] protected float lifeTime;
    [SerializeField] protected int damage;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Transform hitbox;
    [SerializeField] protected bool isAxe, isPickaxe, isHammer;

    protected Timer timerLifeTime;

    public bool IsAxe { get => isAxe; set => isAxe = value; }
    public bool IsPickaxe { get => isPickaxe; set => isPickaxe = value; }
    public int Damage { get => damage; set => damage = value; }

    public virtual void OnObjectSpawnAfter()
    {
        hitbox.gameObject.SetActive(true);
        rb.velocity = rb.transform.right * speed;
        if (isAxe || isPickaxe || isHammer)
        {

        }
        else
        {
            damage = InventoryController.Instance.GetPlayerData.Attack;
        }

        //Debug.Log("Bullet Damage: " + damage);
        timerLifeTime = Timer.DelayAction(lifeTime + (InventoryController.Instance.GetPlayerData.AttackRange / 100f), () =>
        {
            gameObject.SetActive(false);
            OnEndLifeTime();
        });
    }

    public void OnHitEnemy(Collider2D other)
    {
        var enemy = other.GetComponentInParent<Enemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(this);
        }
        OnEndLifeTime();

        ObjectPooler.Instance.SpawnFromPool("player_bullet_impact", rb.transform.position, rb.transform.rotation);

    }

    public void OnHitBuilding(Collider2D other)
    {
        if (isHammer)
        {
            var building = other.GetComponentInParent<BuildingBase>();

            if (building != null)
            {
                building.OnTakeDamage(this);
            }
        }

        OnEndLifeTime();

        ObjectPooler.Instance.SpawnFromPool("player_bullet_impact", rb.transform.position, rb.transform.rotation);
    }

    public void OnEndLifeTime()
    {
        rb.velocity = Vector2.zero;
        hitbox.gameObject.SetActive(false);
    }
}
