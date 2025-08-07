using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, JsonObject(MemberSerialization.OptIn)]

[CreateAssetMenu(fileName = "New Item", menuName = "Farm/Items/Building")]
public class ItemBuilding : ItemBase
{
    [JsonProperty]
    [SerializeField] private string buildingName;
    [SerializeField] private Sprite buildingSprite;

    public string BuildingName { get => buildingName; set => buildingName = value; }
    public Sprite BuildingSprite { get => buildingSprite; set => buildingSprite = value; }
}
