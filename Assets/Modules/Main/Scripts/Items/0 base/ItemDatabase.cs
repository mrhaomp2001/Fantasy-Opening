using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemDatabase : MonoBehaviour
{
    [SerializeField] private static ItemDatabase instance;
    [SerializeField] private List<ItemBase> items;
    [SerializeField] private List<Crop> crop;

    public static ItemDatabase Instance { get => instance; set => instance = value; }
    public List<ItemBase> Items { get => items; set => items = value; }
    public List<Crop> Crop { get => crop; set => crop = value; }

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
}
