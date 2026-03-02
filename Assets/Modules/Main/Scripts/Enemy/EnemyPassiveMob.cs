using GameUtil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPassiveMob : Enemy, IFixedUpdatable
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float moveTime = 2f; // x
    public float idleTime = 1f; // y

    private Vector2 moveDirection;
    private bool isMoving;

    private DelayTimer currentTimer;

    public override void Initialize()
    {
        base.Initialize();
        StartIdle();
    }

    public override void OnDie()
    {
        base.OnDie();
        StopTimer();
        isMoving = false;
    } 

    private void StartIdle()
    {
        isMoving = false;
        StopTimer();
        idleTime = UnityEngine.Random.Range(1f, 4f);
        currentTimer = Timer.DelayAction(idleTime, StartMove, autoDestroyOwner: this);
    }

    private void StartMove()
    {
        isMoving = true;
        moveDirection = GetRandomDirection();

        StopTimer();

        moveTime = UnityEngine.Random.Range(1f, 5f);

        currentTimer = Timer.DelayAction(moveTime, StartIdle, autoDestroyOwner: this);
    }

    private Vector2 GetRandomDirection()
    {
        float angle = UnityEngine.Random.Range(0f, 360f);
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }
    private void UpdateSpriteDirection(Vector2 dir)
    {
        if (dir.x > 0.01f)
        {
            sprite.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (dir.x < -0.01f)
        {
            sprite.localRotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    private void StopTimer()
    {
        if (currentTimer != null)
        {
            currentTimer.Cancel();
            currentTimer = null;
        }
    }

    public void OnFixedUpdate()
    {
        if (isMoving)
        {
            rb.velocity = moveDirection * moveSpeed;
            UpdateSpriteDirection(rb.velocity);
        }
        else
        {
            rb.velocity = Vector2.zero;
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
}
