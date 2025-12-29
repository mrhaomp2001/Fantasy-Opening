using GameUtil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : Enemy, IFixedUpdatable
{
    [Header("EnemyFollow: ")]
    [SerializeField] private bool isMove;
    [SerializeField] private float delayMove;
    [SerializeField] private float speed;
    private Timer timerMoveMent;
    public override void Initialize()
    {
        base.Initialize();
        isMove = false;

        CanMove = true;

        if (timerMoveMent == null)
        {
            timerMoveMent = Timer.DelayAction(Random.Range(0f, delayMove), () =>
            {
                isMove = true;
            });
        }
        else
        {
            timerMoveMent.Restart();
        }
    }

    public void OnFixedUpdate()
    {
        if (CanMove && isMove)
        {
            Move();
            SeparateFromOtherEnemies();
        }
    }

    private void OnEnable()
    {
        UpdateController.Instance.FixedUpdateables.Add(this);
    }

    private void OnDisable()
    {
        UpdateController.Instance.FixedUpdateables.Remove(this);
    }

    private void Move()
    {
        Vector3 playerPosition = PlayerController.Instance.transform.position;
        Vector3 bulletPos = hitbox.position;

        Vector3 directionToPlayer = playerPosition - bulletPos;

        float DesiredHeadingToPlayer = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

        directionToPlayer.x = Mathf.Abs(directionToPlayer.x);
        //directionToPlayer.y = Mathf.Abs(directionToPlayer.y);
        directionToPlayer.z = Mathf.Abs(directionToPlayer.z);

        if (directionToPlayer != Vector3.zero)
        {
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

    [SerializeField] private float separationRadius = 0.5f;
    [SerializeField] private float separationForce = 2f;
    [SerializeField] private LayerMask enemyLayer;

    private void SeparateFromOtherEnemies()
    {
        Vector2 force = Vector2.zero;

        int count = Physics2D.OverlapCircleNonAlloc(
            transform.position,
            enemyCheckRadius,
            enemyHits,
            enemyLayer
        );

        for (int i = 0; i < count; i++)
        {
            var hit = enemyHits[i];
            if (hit == null || hit.transform == transform) continue;

            Vector2 dir = (Vector2)(transform.position - hit.transform.position);
            float dist = dir.magnitude;

            if (dist > 0.001f)
            {
                force += dir.normalized / dist;
            }
        }

        rb.velocity = force.normalized * speed;
    }

    [SerializeField] private float enemyCheckRadius = 0.4f;

    private Collider2D[] enemyHits = new Collider2D[8];

    private bool IsCollidingWithEnemy()
    {
        int count = Physics2D.OverlapCircleNonAlloc(
            transform.position,
            enemyCheckRadius,
            enemyHits,
            enemyLayer
        );

        for (int i = 0; i < count; i++)
        {
            if (enemyHits[i] != null && enemyHits[i].transform != transform)
                return true;
        }

        return false;
    }

}
