using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BuildingController;

public class GameController : MonoBehaviour
{
    private static GameController instance;
    [SerializeField] private List<BuildingFarmland> farmlands;

    public static GameController Instance { get => instance; set => instance = value; }
    public List<BuildingFarmland> Farmlands { get => farmlands; set => farmlands = value; }
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
            foreach (var item in farmlands)
            {
                if (item != null)
                {
                    item.OnNextDay();
                }
            }
        });

    }
}
