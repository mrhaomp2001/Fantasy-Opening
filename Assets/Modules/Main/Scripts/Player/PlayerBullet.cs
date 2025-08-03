using GameUtil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour, IPoolObject
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    [SerializeField] private Rigidbody2D rb;

    private Timer timerLifeTime;

    public void OnObjectSpawnAfter()
    {
        rb.velocity = rb.transform.right * speed;
        timerLifeTime = Timer.DelayAction(lifeTime, () =>
        {
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
    }

    public void OnEndLifeTime()
    {
        Timer.Cancel(timerLifeTime);
        ObjectPooler.Instance.SpawnFromPool("player_bullet_impact", rb.transform.position, rb.transform.rotation);

        gameObject.SetActive(false);
    }
}
