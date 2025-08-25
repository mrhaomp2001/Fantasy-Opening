using Newtonsoft.Json;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    private static BuildingController instance;

    [SerializeField] private Grid gridBuilding;
    [SerializeField] private Tilemap[] tilemaps;

    private LinkedList<IWorldInteractable> interactable;

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

        interactable = new();
    }
    // Interact
    public void OnEnterWorldInteract(Collider2D other)
    {
        interactable.AddFirst(other.GetComponentInParent<IWorldInteractable>());
    }

    public void OnExitWorldInteract(Collider2D other)
    {
        if (interactable.Count > 0)
        {
            interactable.RemoveLast();
        }
    }

    public void OnIndicatorExitBuildng(Collider2D other)
    {
        interactable.RemoveLast();
    }

    public bool IsCellEmpty(Vector3 worldPos)
    {
        Vector3Int cellPosition = gridBuilding.WorldToCell(worldPos);

        foreach (var tilemap in tilemaps)
        {
            if (!tilemap.gameObject.activeInHierarchy) continue;
            if (tilemap.GetTile(cellPosition) != null)
            {
                return false; // đã có tile ở ít nhất 1 tilemap
            }
        }

        return true; // không có tile nào trong tất cả tilemap
    }
    public bool IsBuildValid()
    {
        bool result = true;

        Vector3Int cellPosition = gridBuilding.WorldToCell(PlayerController.Instance.FirepointHitbox.transform.position);
        
        var buildingPosition = gridBuilding.GetCellCenterWorld(cellPosition);

        if (interactable.Count > 0)
        {
            return false;
        }

        if (IsCellEmpty(cellPosition))
        {
            return false;
        }
        
        foreach (var item in InventoryController.Instance.GetPlayerData.BuildingData.Buildings)
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
            Id = InventoryController.Instance.GetPlayerData.BuildingData.IdCounter++,
            Name = buildingName,
            X = buildingPosition.x,
            Y = buildingPosition.y,
            Z = 0,
            WorldInteractable = gameObjectResult.GetComponent<IWorldInteractable>(),
        };

        InventoryController.Instance.GetPlayerData.BuildingData.Buildings.Add(building);

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

        InventoryController.Instance.GetPlayerData.BuildingData.Buildings.Add(building);
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
        foreach (var item in InventoryController.Instance.GetPlayerData.BuildingData.Buildings)
        {
            if (item.WorldInteractable is BuildingFarmland farmland)
            {
                item.Data = farmland;
            }
            if (item.WorldInteractable is BuildingChest chest)
            {
                item.Data = chest;
            }
        }
    }

    public void Load(JSONNode jsonValue)
    {
        InventoryController.Instance.GetPlayerData.BuildingData.IdCounter = jsonValue["idCounter"].AsInt;

        for (int i = 0; i < jsonValue["buildings"].Count; i++)
        {
            var item = jsonValue["buildings"][i];

            if (float.TryParse(item["x"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out float x))
            {
                if (float.TryParse(item["y"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out float y))
                {
                    Vector3 buildPosition = new Vector3(x, y, 0f);

                    var building = BuildAtPosition(item["name"].Value, buildPosition, item["id"].AsInt);

                    var data = item["data"];

                    if (building.WorldInteractable is BuildingFarmland farmland)
                    {
                        if (data["cropCurrent"] != null)
                        {
                            farmland.OnLoadFarmLand(data["cropCurrent"]["id"].AsInt, data["currentDay"].AsInt);
                        }
                    }

                    if (building.WorldInteractable is BuildingChest chest)
                    {
                        for (int j = 0; j < data["items"].Count; j++)
                        {
                            var chestItem = data["items"][j];

                            if (chestItem["item"] != null)
                            {
                                chest.Add(chestItem["item"]["id"].AsInt, chestItem["count"].AsInt);
                            }
                        }
                    }
                }
            }
        }
    }

    public void DestroyBuilding(int id)
    {
        Building targetBuilding = InventoryController.Instance.GetPlayerData.BuildingData.Buildings
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

        InventoryController.Instance.GetPlayerData.BuildingData.Buildings.Remove(targetBuilding);
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
        if (InventoryController.Instance != null)
        {
            if (InventoryController.Instance.GetPlayerData != null && InventoryController.Instance.GetPlayerData.SelectedHotbar != null)
            {
                if (InventoryController.Instance.GetPlayerData.SelectedHotbar.item != null)
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
        }
    }
}
