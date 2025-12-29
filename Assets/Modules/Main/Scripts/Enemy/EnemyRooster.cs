using GameUtil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class EnemyRooster : Enemy
{ 
    [Header("EnemyRooster: ")]
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
                Move();
                SeparateFromOtherEnemies();
            });
        }
        else
        {
            timerMoveMent.Restart();
        }
    }

    private void OnDisable()
    {
        if (timerMoveMent != null)
        {
            timerMoveMent.Cancel();
        }
    }

    [SerializeField] private float separationRadius = 0.5f;
    [SerializeField] private float separationForce = 2f;
    [SerializeField] private LayerMask enemyLayer;

    private void SeparateFromOtherEnemies()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            separationRadius,
            enemyLayer
        );

        Vector2 force = Vector2.zero;

        foreach (var hit in hits)
        {
            if (hit.transform == transform) continue;

            Vector2 dir = (Vector2)(transform.position - hit.transform.position);
            float distance = dir.magnitude;

            if (distance > 0)
            {
                force += dir.normalized / distance;
            }
        }

        rb.AddForce(force * separationForce, ForceMode2D.Force);
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
