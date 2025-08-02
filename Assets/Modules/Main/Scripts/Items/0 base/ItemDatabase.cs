using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemDatabase : MonoBehaviour
{
    [SerializeField] private static ItemDatabase instance;
    [SerializeField] private List<ItemBase> items;

    public static ItemDatabase Instance { get => instance; set => instance = value; }
    public List<ItemBase> Items { get => items; set => items = value; }

    private void Start()
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
