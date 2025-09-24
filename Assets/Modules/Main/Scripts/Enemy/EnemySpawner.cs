using GameUtil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private string enemyName;
    private GameObject enemy;

    public string EnemyName { get => enemyName; set => enemyName = value; }

    private void Start()
    {
        SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        //Debug.Log($"Spawn enemy: {gameObject.name}, enemy name: {enemyName}", gameObject);
        enemy = ObjectPooler.Instance.SpawnFromPool(enemyName, transform.position, Quaternion.identity);
    }

    public void ResetEnemy()
    {
        if (enemy != null)
        {
            enemy.SetActive(false);
        }
        enemy = null;
    }
}
