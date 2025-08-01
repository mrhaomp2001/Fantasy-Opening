using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<Farmland> farmlands;
    
    public List<Farmland> Farmlands { get => farmlands; set => farmlands = value; }

    public void NextDay()
    {
        foreach (var item in farmlands)
        {
            item.OnNextDay();
        }
    }
}
