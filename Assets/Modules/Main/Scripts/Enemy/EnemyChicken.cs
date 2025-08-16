using GameUtil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChicken : Enemy, IFixedUpdatable
{
    [Header("EnemyChicken: ")]
    [SerializeField] private bool isMove;
    [SerializeField] private float delayMove;
    [SerializeField] private float speed;
    private Timer timerMoveMent;
    [SerializeField] private Rigidbody2D rb;
    public override void Initialize()
    {
        base.Initialize();
        isMove = false;

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
        if (isMove)
        {
            Move();
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
}
