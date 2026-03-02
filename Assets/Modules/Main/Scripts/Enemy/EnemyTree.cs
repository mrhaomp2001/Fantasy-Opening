using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                if (playerBulletInput.ItemDropWhenEnemyHited != 0)
                {
                    for (int i = 0; i < playerBulletInput.ItemDropWhenEnemyHitedCount; i++)
                    {
                        WorldItemController.Instance.SpawnItem(playerBulletInput.ItemDropWhenEnemyHited, rb.transform.position, 1);
                    }

                    if (playerBulletInput.ItemDropWhenEnemyHited == 430)
                    {
                        var tech = WitchSystemController.Instance.Data.WitchTechnologies
                        .Where((predicate) =>
                        {
                            return predicate.Id == 3;
                        })
                        .FirstOrDefault();

                        if (tech.Level >= 1)
                        {
                            WorldItemController.Instance.SpawnItem(playerBulletInput.ItemDropWhenEnemyHited, rb.transform.position, 1);
                        }
                    }
                }

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
