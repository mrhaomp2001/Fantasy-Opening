using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletForwardPlayer : EnemyBullet
{
    [SerializeField] protected float speed;
    protected override void MoveMethod(LeanTweenType leanEasing = LeanTweenType.notUsed)
    {
       // Debug.Log("MoveMethod: " + 1);

        Vector3 playerPosition = PlayerController.Instance.transform.position;
        Vector3 bulletPos = bullet.position;

        Vector3 directionToPlayer = playerPosition - bulletPos;

        float DesiredHeadingToPlayer = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

        directionToPlayer.x = Mathf.Abs(directionToPlayer.x);
        //directionToPlayer.y = Mathf.Abs(directionToPlayer.y);
        directionToPlayer.z = Mathf.Abs(directionToPlayer.z);

        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

        bullet.rotation = Quaternion.Euler(0f, 0f, DesiredHeadingToPlayer);

        rb.velocity = bullet.right * speed;

    }

    public override void Despawn()
    {
        base.Despawn();

        rb.velocity = Vector2.zero;
    }
}
