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

    public void ResetEnemy()
    {
        if (enemy != null)
        {
            if (enemy.activeSelf)
            {
                enemy.gameObject.SetActive(false);
            }

            enemy = null;
        }
    }

    private void OnEnable()
    {
        if (GameController.Instance != null)
        {
            GameController.Instance.EnemySpawners.Add(this);
        }
    }

    private void OnDisable()
    {
        if (GameController.Instance != null)
        {
            GameController.Instance.EnemySpawners.Remove(this);
        }
    }
}
