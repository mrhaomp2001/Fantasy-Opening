using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "New Item", menuName = "Farm/Items/Boss Summon")]

public class ItemBossSummon : ItemBase
{
    [SerializeField] private string bossName;

    public string BossName { get => bossName; set => bossName = value; }

    public void OnSummonBoss()
    {
        InventoryController.Instance.Consume(Id, 1, new Callback
        {
            onSuccess = () =>
            {

                Vector2 randomDirection = Random.insideUnitCircle.normalized;
                Vector2 spawnPosition = (Vector2)PlayerController.Instance.RbPlayer.position + randomDirection * 7f;

                GameObject enemyGameObject = ObjectPooler.Instance.SpawnFromPool(bossName, spawnPosition, Quaternion.identity);

                var enemy = enemyGameObject.GetComponent<Enemy>();

                GameController.Instance.MoringWaveEnemy.Add(enemy);

                if (GameController.Instance.MoringWaveEnemy != null)
                {
                    GameController.Instance.UpdateEnemyMaxHp();
                    GameController.Instance.UpdateEnemyHealth();
                }

            },
            onFail = (message) =>
            {

            },
            onNext = () =>
            {

            }
        });
    }
}
