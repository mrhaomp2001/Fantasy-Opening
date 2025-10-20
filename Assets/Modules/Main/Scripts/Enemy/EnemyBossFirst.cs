using GameUtil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossFirst : EnemyBoss
{
    [Header("EnemyBossFirst: ")]
    [SerializeField] private float delayMove;
    [SerializeField] private float speed;
    private Timer timerMoveMent;
    public override void Initialize()
    {
        base.Initialize();

        if (timerMoveMent == null)
        {
            timerMoveMent = Timer.LoopAction(delayMove, (int count) =>
            {
                if (Random.Range((int)0, 3) == 0)
                {
                    CanMove = true;
                    Move();
                }
                else
                {
                    CanMove = false;

                    FireBullets();
                }
            });
        }
        else
        {
            timerMoveMent.Restart();
        }
    }

    private void FireBullets()
    {
        for (int i = 0; i < 7; i++)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            Vector2 spawnPosition = (Vector2)transform.position + randomDirection * 2f;

            GameObject bulletObject = ObjectPooler.Instance.SpawnFromPool("boss_1_bullet_2", spawnPosition, Quaternion.identity);

            var bullet = bulletObject.GetComponent<EnemyBulletForwardPlayer>();

            //bullet.SetBulletColor(new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f)));

            bullet.StartMove();
        }
    }

    private void OnDisable()
    {
        if (timerMoveMent != null)
        {
            timerMoveMent.Cancel();
        }
    }

    private void Move()
    {
        if (CanMove)
        {
            Vector3 playerPosition = PlayerController.Instance.transform.position;
            Vector3 bulletPos = hitbox.position;

            Vector3 directionToPlayer = playerPosition - bulletPos;

            float DesiredHeadingToPlayer = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

            directionToPlayer.x = Mathf.Abs(directionToPlayer.x);
            //directionToPlayer.y = Mathf.Abs(directionToPlayer.y);
            directionToPlayer.z = Mathf.Abs(directionToPlayer.z);

            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

            hitbox.rotation = Quaternion.Euler(0f, 0f, DesiredHeadingToPlayer);

            rb.velocity = hitbox.right * speed;

            if (transform.position.x > playerPosition.x)
            {
                sprite.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else
            {
                sprite.rotation = Quaternion.Euler(0f, 0f, 0f);

            }
        }
    }
}
