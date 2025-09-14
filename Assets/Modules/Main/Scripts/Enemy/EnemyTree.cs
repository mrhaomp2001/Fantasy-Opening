using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTree : Enemy
{
    public override void Initialize()
    {
        base.Initialize();

    }

    public override void Despawn()
    {
        
    }

    public override void TakeDamage(PlayerBullet playerBulletInput)
    {
        if (playerBulletInput != null)
        {
            if (playerBulletInput.IsAxe)
            {
                Hp -= Mathf.Max(0, playerBulletInput.Damage - Def);

                if (Animator != null && Mathf.Max(0, playerBulletInput.Damage - Def) > 0)
                {
                    Animator.Play("hurt");
                }

                if (Hp <= 0)
                {
                    OnDie();
                }

                if (GameController.Instance.MoringWaveEnemy != null)
                {
                    GameController.Instance.UpdateEnemyHealth();
                }
            }
        }
    }
}
