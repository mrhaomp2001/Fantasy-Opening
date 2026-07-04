using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dimension
{
    [SerializeField] private string name;
    [SerializeField] private Transform container;

    public string Name { get => name; set => name = value; }
    public Transform Container { get => container; set => container = value; }
}
public class DimensionController : Singleton<DimensionController>
{
    [SerializeField] private string currentDimension;
    [SerializeField] private List<Dimension> dimensions;

    public string CurrentDimension { get => currentDimension; set => currentDimension = value; }

    private void Start()
    {
        DimensionController.Instance.ChangeDimension("overworld");
    }

    public void ChangeDimension(string dimensionName)
    {
        foreach (var item in dimensions)
        {
            item.Container.gameObject.SetActive(false);
        }

        var dimension = dimensions.Find(x => x.Name == dimensionName);
        if (dimension != null)
        {
            Debug.Log($"Changing to dimension: {dimensionName}");

            dimension.Container.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"Dimension '{dimensionName}' not found.");
        }

        currentDimension = dimensionName;

        for (int i = 0; i < InventoryController.Instance.GetPlayerData.BuildingData.Buildings.Count; i++)
        {
            var item = InventoryController.Instance.GetPlayerData.BuildingData.Buildings[i];

            if (item != null && item.WorldInteractable != null)
            {
                if (item.WorldInteractable is BuildingBase buildingBase)
                {
                    if (item.Dimension.Equals(dimensionName))
                    {
                        if (buildingBase is BuildingFoundation foundation)
                        {
                            foundation.BuildTile();
                        }
                        else
                        {
                            buildingBase.gameObject.SetActive(true);

                        }
                    }
                    else
                    {
                        if (buildingBase is BuildingFoundation foundation)
                        {
                            foundation.RemoveTile();
                        }
                        else
                        {
                            buildingBase.gameObject.SetActive(false);

                        }
                    }
                }
            }
        }

        EnemySpawner[] enemySpawners = FindObjectsByType<EnemySpawner>(findObjectsInactive: FindObjectsInactive.Include);

        for (int i = 0; i < enemySpawners.Length; i++)
        {
            var item = enemySpawners[i];
            item.OnChangeDimension();

        }
}
}