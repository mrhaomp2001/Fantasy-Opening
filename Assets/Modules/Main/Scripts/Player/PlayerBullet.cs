using GameUtil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour, IPoolObject
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    [SerializeField] private int damage;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform hitbox;
    [SerializeField] private bool isAxe, isPickaxe;

    private Timer timerLifeTime;

    public bool IsAxe { get => isAxe; set => isAxe = value; }
    public bool IsPickaxe { get => isPickaxe; set => isPickaxe = value; }
    public int Damage { get => damage; set => damage = value; }

    public void OnObjectSpawnAfter()
    {
        hitbox.gameObject.SetActive(true);
        rb.velocity = rb.transform.right * speed;
        damage = InventoryController.Instance.GetPlayerData.Attack;
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

    public void OnEndLifeTime()
    {
        rb.velocity = Vector2.zero;
        hitbox.gameObject.SetActive(false);
    }
}
