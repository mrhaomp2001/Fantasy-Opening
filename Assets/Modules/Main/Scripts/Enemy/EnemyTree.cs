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
                base.TakeDamage(playerBulletInput);
            }
        }
    }
}
