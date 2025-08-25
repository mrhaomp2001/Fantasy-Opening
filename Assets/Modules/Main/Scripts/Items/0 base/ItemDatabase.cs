using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExpPerLevel
{
    [SerializeField] private int level;
    [SerializeField] private int expNeeded;

    public int Level { get => level; set => level = value; }
    public int ExpNeeded { get => expNeeded; set => expNeeded = value; }

}

public class ItemDatabase : MonoBehaviour
{
    [SerializeField] private static ItemDatabase instance;
    [SerializeField] private List<ItemBase> items;
    [SerializeField] private List<Crop> crop;
    [SerializeField] private List<ExpPerLevel> expPerLevels;
    [SerializeField] private List<BuffBase> buffs;
    public static ItemDatabase Instance { get => instance; set => instance = value; }
    public List<ItemBase> Items { get => items; set => items = value; }
    public List<Crop> Crop { get => crop; set => crop = value; }
    public List<ExpPerLevel> ExpPerLevels { get => expPerLevels; set => expPerLevels = value; }
    public List<BuffBase> Buffs { get => buffs; set => buffs = value; }

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
