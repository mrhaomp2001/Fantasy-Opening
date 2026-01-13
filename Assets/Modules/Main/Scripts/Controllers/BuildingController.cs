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
        [SerializeField] private string x, y, z;
        [JsonProperty]
        [SerializeField] private object data = new();
        [SerializeField] private IWorldInteractable worldInteractable;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string X { get => x; set => x = value; }
        public string Y { get => y; set => y = value; }
        public string Z { get => z; set => z = value; }
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
    [SerializeField] private Tilemap tilemapPlayerBuild;
    [SerializeField] private Tilemap[] tilemaps;


    private LinkedList<IWorldInteractable> interactable;

    public static BuildingController Instance { get => instance; set => instance = value; }

    public static bool TryParseFloatInvariant(string raw, out float value)
    {
        value = 0f;

        if (string.IsNullOrEmpty(raw))
            return false;

        // Cứu dữ liệu có dấu phẩy (5,5 → 5.5)
        raw = raw.Replace(',', '.');

        return float.TryParse(
            raw,
            NumberStyles.Float,
            CultureInfo.InvariantCulture,
            out value
        );
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

    public bool IsCellEmpty()
    {
        Vector3Int cellPosition = gridBuilding.WorldToCell(PlayerController.Instance.FirepointHitbox.transform.position);

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

        if (IsCellEmpty())
        {
            return false;
        }

        foreach (var item in InventoryController.Instance.GetPlayerData.BuildingData.Buildings)
        {
            if (item.WorldInteractable is BuildingFoundation)
            {
                continue;
            }

            TryParseFloatInvariant(item.X, out float x);
            TryParseFloatInvariant(item.Y, out float y);
            TryParseFloatInvariant(item.Z, out float z);

            Vector3 checkPosition = new Vector3(x, y, z);

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

       // Debug.Log($"x={buildingPosition.x}, y={buildingPosition.y}, z={buildingPosition.z}");

        var building = new Building
        {
            Id = InventoryController.Instance.GetPlayerData.BuildingData.IdCounter++,
            Name = buildingName,
            X = buildingPosition.x.ToString(CultureInfo.InvariantCulture),
            Y = buildingPosition.y.ToString(CultureInfo.InvariantCulture),
            Z = "0",
            WorldInteractable = gameObjectResult.GetComponent<IWorldInteractable>(),
        };

        InventoryController.Instance.GetPlayerData.BuildingData.Buildings.Add(building);

        if (building.WorldInteractable is BuildingBase buildingBase)
        {
            if (buildingBase != null)
            {
                buildingBase.Id = building.Id;
            }

            buildingBase.Initialize();
        }


        if (building.WorldInteractable is BuildingFarmland farmland)
        {
            farmland.Id = building.Id;
            GameController.Instance.Farmlands.Add(farmland);
        }

        return building;
    }

    public void BuildTile(Vector3 position, TileBase tile)
    {
        Vector3Int cellPosition = tilemapPlayerBuild.WorldToCell(position);

        tilemapPlayerBuild.SetTile(cellPosition, tile);
    }

    public Building BuildAtPosition(string buildingName, Vector3 position, int buildingId)
    {
        Vector3Int cellPosition = gridBuilding.WorldToCell(position);

        var buildingPosition = gridBuilding.GetCellCenterWorld(cellPosition);

        GameObject gameObjectResult = ObjectPooler.Instance.SpawnFromPool(buildingName, buildingPosition, Quaternion.identity);

       // Debug.Log($"x={buildingPosition.x}, y={buildingPosition.y}, z={buildingPosition.z}");

        var building = new Building
        {
            Id = buildingId,
            Name = buildingName,
            X = buildingPosition.x.ToString(CultureInfo.InvariantCulture),
            Y = buildingPosition.y.ToString(CultureInfo.InvariantCulture),
            Z = buildingPosition.z.ToString(CultureInfo.InvariantCulture),
            WorldInteractable = gameObjectResult.GetComponent<IWorldInteractable>(),
        };

        InventoryController.Instance.GetPlayerData.BuildingData.Buildings.Add(building);

        if (building.WorldInteractable is BuildingBase buildingBase)
        {
            if (buildingBase != null)
            {
                buildingBase.Id = building.Id;
            }

            buildingBase.Initialize();
        }


        if (building.WorldInteractable is BuildingFarmland farmland)
        {
            GameController.Instance.Farmlands.Add(farmland);
        }

        return building;
    }

    public void Save()
    {
       // Debug.Log("Save: 1");
        foreach (var item in InventoryController.Instance.GetPlayerData.BuildingData.Buildings)
        {
           // Debug.Log("Save: 2");

            if (item.WorldInteractable is BuildingFarmland farmland)
            {
                item.Data = farmland;
            }
            if (item.WorldInteractable is BuildingChest chest)
            {
                item.Data = chest;
            }
            if (item.WorldInteractable is BuildingBase buildingBase)
            {
                buildingBase.NextDay();
            }
        }

       // Debug.Log($"Buildings: {JsonConvert.SerializeObject(InventoryController.Instance.GetPlayerData.BuildingData.Buildings)}");

       // Debug.Log("Save: 3");

    }

    public void Load(JSONNode jsonValue)
    {
        //Debug.Log("Load: 1");

        InventoryController.Instance.GetPlayerData.BuildingData.IdCounter = jsonValue["idCounter"].AsInt;

        //Debug.Log("Load: 2");
        //Debug.Log($"Load value: {jsonValue["buildings"].ToString()}");

        for (int i = 0; i < jsonValue["buildings"].Count; i++)
        {
            var item = jsonValue["buildings"][i];

            if (float.TryParse(item["x"].Value.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out float x))
            {
                //Debug.Log($"Load: x = {x}");
                //Debug.Log($"Load: x value = {item["x"].Value.ToString()}");

                if (float.TryParse(item["y"].Value.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out float y))
                {
                    //Debug.Log($"Load: y = {y}");
                    //Debug.Log($"Load: y value = {item["y"].Value.ToString()}");

                    //Debug.Log("--");

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

       // Debug.Log("Load: 3");

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
                building.OnDestroyBuilding();

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

                    if (InventoryController.Instance.GetPlayerData.SelectedHotbar.item is ItemBuildingFoundation buildingFoundation)
                    {
                        Vector3Int cellPosition = gridBuilding.WorldToCell(PlayerController.Instance.FirepointHitbox.transform.position);

                        PopUpInventory.Instance.TransformBuildingIndicator.transform.position = gridBuilding.GetCellCenterWorld(cellPosition);

                        if (IsCellEmpty())
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
