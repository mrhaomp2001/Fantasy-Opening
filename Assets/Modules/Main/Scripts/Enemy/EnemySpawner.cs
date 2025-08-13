using GameUtil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private string enemyName;
    private GameObject enemy;

    private void Start()
    {
        SpawnEnemy();

        GameController.Instance.EnemySpawners.Add(this);
    }

    public void SpawnEnemy()
    {
        if (enemy == null || enemy.activeSelf == false)
        {
            enemy = ObjectPooler.Instance.SpawnFromPool(enemyName, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning($"Enemy already spawned: {enemyName}");
        }
    }
}
