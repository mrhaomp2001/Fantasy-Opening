using GameUtil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour, IPoolObject
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform hitbox;

    private Timer timerLifeTime;

    public void OnObjectSpawnAfter()
    {
        hitbox.gameObject.SetActive(true);
        rb.velocity = rb.transform.right * speed;
        timerLifeTime = Timer.DelayAction(lifeTime, () =>
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
            enemy.TakeDamage();
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
