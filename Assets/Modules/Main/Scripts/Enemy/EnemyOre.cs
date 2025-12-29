using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOre : Enemy
{
    public override void Despawn()
    {

    }

    public override void TakeDamage(PlayerBullet playerBulletInput)
    {
        if (playerBulletInput != null)
        {
            if (playerBulletInput.IsPickaxe)
            {
                Hp -= Mathf.Max(0, playerBulletInput.Damage - Def);

                if (Animator != null && Mathf.Max(0, playerBulletInput.Damage - Def) > 0)
                {
                    Animator.Play("hurt");
                    PlayHurtAudio();
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
