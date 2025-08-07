using Newtonsoft.Json;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using static InventoryController;

public class BuildingController : MonoBehaviour, IUpdatable
{

    [System.Serializable, JsonObject(MemberSerialization.OptIn)]
    public class Building
    {
        [JsonProperty]
        [SerializeField] private int id;
        [JsonProperty]
        [SerializeField] private string name;
        [JsonProperty]
        [SerializeField] private float x, y, z;
        [JsonProperty]
        [SerializeField] private object data = new();
        [SerializeField] private IWorldInteractable worldInteractable;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public float X { get => x; set => x = value; }
        public float Y { get => y; set => y = value; }
        public float Z { get => z; set => z = value; }
        public object Data { get => data; set => data = value; }
        public IWorldInteractable WorldInteractable { get => worldInteractable; set => worldInteractable = value; }
    }

    [System.Serializable, JsonObject(MemberSerialization.OptIn)]
    public class BuildingData
    {
        [JsonProperty]
        [SerializeField] private int idCounter;
        [JsonProperty]
        [SerializeField] private List<Building> buildings = new();

        public int IdCounter { get => idCounter; set => idCounter = value; }
        public List<Building> Buildings { get => buildings; set => buildings = value; }
    }

    [System.Serializable]
    public class BuildingBound
    {
        public Transform downLeft;
        public Transform upRight;

        public bool Contains(Vector3 target)
        {
            Vector3 pos = target;

            return pos.x >= downLeft.position.x && pos.x <= upRight.position.x &&
                   pos.y >= downLeft.position.y && pos.y <= upRight.position.y &&
                   pos.z >= downLeft.position.z && pos.z <= upRight.position.z;
        }
    }

    private static BuildingController instance;

    [SerializeField] private Grid gridBuilding;
    [SerializeField] private BuildingData buildingData;

    [SerializeField] private List<BuildingBound> allowedBuildAreas = new List<BuildingBound>();

    private const string prefKey = "BuildingController";

    public static BuildingController Instance { get => instance; set => instance = value; }

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
    public bool IsBuildValid()
    {
        bool result = true;

        Vector3Int cellPosition = gridBuilding.WorldToCell(PlayerController.Instance.FirepointHitbox.transform.position);

        var buildingPosition = gridBuilding.GetCellCenterWorld(cellPosition);

        bool isInsideAllowedArea = false;
        foreach (var bound in allowedBuildAreas)
        {
            if (bound.Contains(buildingPosition))
            {
                isInsideAllowedArea = true;
                break;
            }
        }

        if (!isInsideAllowedArea)
        {
            return false;
        }

        foreach (var item in buildingData.Buildings)
        {
            Vector3 checkPosition = new Vector3(item.X, item.Y, item.Z);

            if (buildingPosition.Equals(checkPosition))
            {
                result = false;
                break;
            }
        }
        return result;
    }

    public Building Build(string buildingName)
    {
        Vector3Int cellPosition = gridBuilding.WorldToCell(PlayerController.Instance.FirepointHitbox.transform.position);

        var buildingPosition = gridBuilding.GetCellCenterWorld(cellPosition);
        buildingPosition.z = 0;

        GameObject gameObjectResult = ObjectPooler.Instance.SpawnFromPool(buildingName, buildingPosition, Quaternion.identity);

        var building = new Building
        {
            Id = buildingData.IdCounter++,
            Name = buildingName,
            X = buildingPosition.x,
            Y = buildingPosition.y,
            Z = 0,
            WorldInteractable = gameObjectResult.GetComponent<IWorldInteractable>(),
        };

        buildingData.Buildings.Add(building);

        var buildingBase = gameObjectResult.GetComponent<BuildingBase>();

        if (buildingBase != null)
        {
            buildingBase.Id = building.Id;
        }

        if (building.WorldInteractable is BuildingFarmland farmland)
        {
            farmland.Id = building.Id;
            GameController.Instance.Farmlands.Add(farmland);
        }

        return building;
    }

    public Building BuildAtPosition(string buildingName, Vector3 position, int buildingId)
    {
        Vector3Int cellPosition = gridBuilding.WorldToCell(position);

        var buildingPosition = gridBuilding.GetCellCenterWorld(cellPosition);

        GameObject gameObjectResult = ObjectPooler.Instance.SpawnFromPool(buildingName, buildingPosition, Quaternion.identity);

        var building = new Building
        {
            Id = buildingId,
            Name = buildingName,
            X = buildingPosition.x,
            Y = buildingPosition.y,
            Z = buildingPosition.z,
            WorldInteractable = gameObjectResult.GetComponent<IWorldInteractable>(),
        };

        buildingData.Buildings.Add(building);
        var buildingBase = gameObjectResult.GetComponent<BuildingBase>();

        if (buildingBase != null)
        {
            buildingBase.Id = building.Id;
        }

        if (building.WorldInteractable is BuildingFarmland farmland)
        {
            GameController.Instance.Farmlands.Add(farmland);
        }

        return building;
    }

    public void Save()
    {
        foreach (var item in buildingData.Buildings)
        {
            if (item.WorldInteractable is BuildingFarmland farmland)
            {
                item.Data = farmland;
            }
        }

        Debug.Log($"{JsonConvert.SerializeObject(buildingData)}");
        PlayerPrefs.SetString(prefKey, JsonConvert.SerializeObject(buildingData));
        PlayerPrefs.Save();
    }

    public void Load()
    {
        buildingData = new BuildingData
        {
            IdCounter = 0,
            Buildings = new List<Building>()
        };

        if (PlayerPrefs.HasKey(prefKey))
        {
            string value = PlayerPrefs.GetString(prefKey);

            JSONNode keyValuePairs = JSONNode.Parse(value);

            Debug.Log($"OnLoadPrefs: {value}");

            buildingData.IdCounter = keyValuePairs["idCounter"].AsInt;

            for (int i = 0; i < keyValuePairs["buildings"].Count; i++)
            {
                var item = keyValuePairs["buildings"][i];

                if (float.TryParse(item["x"].Value, System.Globalization.NumberStyles.Float, CultureInfo.InvariantCulture, out float x))
                {
                    if (float.TryParse(item["y"].Value, System.Globalization.NumberStyles.Float, CultureInfo.InvariantCulture, out float y))
                    {

                        Vector3 buildPosition = new Vector3(x, y, 0f);

                        var building = BuildAtPosition(item["name"].Value, buildPosition, item["id"].AsInt);

                        if (building.WorldInteractable is BuildingFarmland farmland)
                        {
                            var data = item["data"];
                            if (data["cropCurrent"] != null)
                            {
                                farmland.OnLoadFarmLand(data["cropCurrent"]["id"].AsInt, data["currentDay"].AsInt);
                            }
                        }
                    }
                }



            }
        }
    }

    public void DestroyBuilding(int id)
    {
        Building targetBuilding = buildingData.Buildings
            .Where(predicate =>
            {
                return predicate.Id == id;
            })
            .FirstOrDefault();

        if (targetBuilding != null)
        {
            if (targetBuilding.WorldInteractable is BuildingBase building)
            {
                building.gameObject.SetActive(false);
            }
        }

        buildingData.Buildings.Remove(targetBuilding);
    }
    private void OnEnable()
    {
        UpdateController.Instance.Updatables.Add(this);
    }

    private void OnDisable()
    {
        UpdateController.Instance.Updatables.Remove(this);
    }

    public void OnUpdate()
    {
        if (InventoryController.Instance.GetPlayerData.SelectedHotbar.item is ItemBuilding building)
        {
            Vector3Int cellPosition = gridBuilding.WorldToCell(PlayerController.Instance.FirepointHitbox.transform.position);

            PopUpInventory.Instance.TransformBuildingIndicator.transform.position = gridBuilding.GetCellCenterWorld(cellPosition);

            if (IsBuildValid())
            {
                PopUpInventory.Instance.TransformBuildingIndicator.color = Color.green;
            }
            else
            {
                PopUpInventory.Instance.TransformBuildingIndicator.color = Color.red;
            }
        }
    }
}
