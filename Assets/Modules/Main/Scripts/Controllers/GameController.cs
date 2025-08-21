using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController instance;
    [SerializeField] private List<BuildingFarmland> farmlands;
    [SerializeField] private List<EnemySpawner> enemySpawners;
    public static GameController Instance { get => instance; set => instance = value; }
    public List<BuildingFarmland> Farmlands { get => farmlands; set => farmlands = value; }
    public List<EnemySpawner> EnemySpawners { get => enemySpawners; set => enemySpawners = value; }

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
    }
    public void NextDay()
    {
        PopUpTransition.Instance.StartTransition(() =>
        {
            FarmlandNextDay();
            EnemyNextDay();
            ProgressionNextDay();

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
        var progresstion = ProgressionController.Instance.Progressions
        .Where(predicate =>
        {
            return predicate.ProgressionName.Equals("event_3");
        })
        .FirstOrDefault();

        progresstion.OnSave();
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
