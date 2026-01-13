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

    [Header("EXP Curve Settings")]
    [SerializeField] private int maxLevel = 100;
    [SerializeField] private int baseExp = 10;
    [SerializeField, Tooltip("Giá trị 1.5 = cong vừa, 2 = cong mạnh, 1.2 = cong nhẹ")]
    private float growthCurve = 1.5f;

    [ContextMenu("Generate EXP Curve Table")]
    private void GenerateExpTable()
    {
        expPerLevels.Clear();

        for (int level = 1; level <= maxLevel; level++)
        {
            // công thức đường cong
            int exp = Mathf.RoundToInt(baseExp * Mathf.Pow(level, growthCurve));

            expPerLevels.Add(new ExpPerLevel
            {
                Level = level,
                ExpNeeded = exp
            });
        }

       // Debug.Log($"Generated EXP curve table (1 → {maxLevel}) using growth {growthCurve}");
    }

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

    private void Start()
    {
        GenerateExpTable();
    }
}
