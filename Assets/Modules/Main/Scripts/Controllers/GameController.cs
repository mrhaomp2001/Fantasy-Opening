using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<Farmland> farmlands;
    [SerializeField] private List<Transform> transformEnemySpawnPositions;
    
    public List<Farmland> Farmlands { get => farmlands; set => farmlands = value; }

    public void NextDay()
    {
        PopUpTransition.Instance.StartTransition(() =>
        {
            foreach (var item in farmlands)
            {
                item.OnNextDay();
            }
            foreach (var item in transformEnemySpawnPositions)
            {
                ObjectPooler.Instance.SpawnFromPool("enemy_1", item.position, Quaternion.identity);
                ObjectPooler.Instance.SpawnFromPool("enemy_2", item.position, Quaternion.identity);
            }
            InventoryController.Instance.OnSavePrefs();
        });

    }
}
