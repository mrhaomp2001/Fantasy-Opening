using GameUtil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet612 : PlayerBullet
{
    [SerializeField] private float accelerationTime = 0.5f; // thời gian để đạt tốc độ max
    [SerializeField] private float startSpeedFactor = 0.2f; // phần trăm tốc độ ban đầu
    Timer accelerationTimer;
    public override void OnObjectSpawnAfter()
    {
        hitbox.gameObject.SetActive(true);

        if (isAxe || isPickaxe || isHammer)
        {
            // tool logic
        }
        else
        {
            damage = InventoryController.Instance.GetPlayerData.Attack;
        }

        float startSpeed = speed * startSpeedFactor;

        if (accelerationTimer != null)
        {
            Timer.Cancel(accelerationTimer);
        }

        accelerationTimer = Timer.DelayAction(accelerationTime,
            onComplete: () =>
            {
                // giữ nguyên tốc độ sau khi hoàn tất
                rb.velocity = rb.transform.right * speed;
            },
            onUpdate: (t) =>
            {
                // t: từ 0 -> 1 theo tiến trình
                float currentSpeed = Mathf.Lerp(startSpeed, speed, t);
                rb.velocity = rb.transform.right * currentSpeed;
            });

        timerLifeTime = Timer.DelayAction(lifeTime + (InventoryController.Instance.GetPlayerData.AttackRange / 100f), () =>
        {
            gameObject.SetActive(false);
            OnEndLifeTime();
        });
    }
}
