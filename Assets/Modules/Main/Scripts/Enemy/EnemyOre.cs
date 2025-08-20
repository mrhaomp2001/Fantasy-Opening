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
                base.TakeDamage(playerBulletInput);
            }
        }
    }
}
