using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private static GameController instance;
    [SerializeField] private List<BuildingFarmland> farmlands;
    [SerializeField] private List<EnemySpawner> enemySpawners;
    [SerializeField] private List<Enemy> moringWaveEnemy;
    [SerializeField] private RectTransform enemyHealthContainer;
    [SerializeField] private Slider sliderTotalEnemyHealth;

    [SerializeField] private int totalEnemyHealth;

    public static GameController Instance { get => instance; set => instance = value; }
    public List<BuildingFarmland> Farmlands { get => farmlands; set => farmlands = value; }
    public List<EnemySpawner> EnemySpawners { get => enemySpawners; set => enemySpawners = value; }
    public List<Enemy> MoringWaveEnemy { get => moringWaveEnemy; set => moringWaveEnemy = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        moringWaveEnemy = new List<Enemy>();
    }
    public void NextDay()
    {
        PopUpTransition.Instance.StartTransition(() =>
        {
            FarmlandNextDay();
            EnemyNextDay();
            ProgressionNextDay();
            UpdateEnemyHealth();

            InventoryController.Instance.Save();
        });


    }



    private void FarmlandNextDay()
    {
        foreach (var item in farmlands)
        {
            if (item != null)
            {
                item.OnNextDay();
            }
        }
    }

    private void ProgressionNextDay()
    {
        enemyHealthContainer.gameObject.SetActive(true);
        totalEnemyHealth = 0;

        for (int i = 0; i < MoringWaveEnemy.Count; i++)
        {
            var enemy = MoringWaveEnemy[i];
            totalEnemyHealth += enemy.HpMax;
        }

        sliderTotalEnemyHealth.maxValue = totalEnemyHealth;

    }

    public void UpdateEnemyHealth()
    {
        int remainHp = 0;
        for (int i = 0; i < MoringWaveEnemy.Count; i++)
        {
            var enemy = MoringWaveEnemy[i];
            remainHp += enemy.Hp;
        }
        sliderTotalEnemyHealth.value = remainHp;
        
        if (remainHp <= 0)
        {
            enemyHealthContainer.gameObject.SetActive(false);
        }
        else
        {
            enemyHealthContainer.gameObject.SetActive(true);

        }
    }
    public void OnLevelUp()
    {
        foreach (var item in moringWaveEnemy)
        {
            item.CanMove = false;
        }

        PopUpUpgradeSelector.Instance.ShowUpgrade();
    }

    public void OnSelectedBuff(BuffBase buff)
    {
        foreach (var item in moringWaveEnemy)
        {
            item.CanMove = true;
        }

        InventoryController.Instance.GetPlayerData.Buffs.Add(buff);
    }

    private void EnemyNextDay()
    {
        foreach (var item in enemySpawners)
        {
            if (item != null)
            {
                item.ResetEnemy();
            }
        }

        foreach (var item in enemySpawners)
        {
            if (item != null)
            {
                item.SpawnEnemy();
            }
        }
    }
}
