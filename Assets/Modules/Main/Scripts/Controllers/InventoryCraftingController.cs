using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ingredient
{
    [SerializeField] private ItemBase itemInput;
    [SerializeField] private int count;

    public ItemBase ItemInput { get => itemInput; set => itemInput = value; }
    public int Count { get => count; set => count = value; }
}

[System.Serializable]
public class Recipe
{
    [SerializeField] private int resultCount;
    [SerializeField] private ItemBase itemResult;
    [SerializeField] private List<Ingredient> ingredients;

    public ItemBase ItemResult { get => itemResult; set => itemResult = value; }
    public List<Ingredient> Ingredients { get => ingredients; set => ingredients = value; }
    public int ResultCount { get => resultCount; set => resultCount = value; }
}

[JsonObject(MemberSerialization.OptIn), System.Serializable]
public class RecipeWithCondition
{
    [JsonProperty]
    [SerializeField] private int id;
    [JsonProperty]
    [SerializeField] private bool isUnlocked;
    [SerializeField] private Recipe recipe;

    public int Id { get => id; set => id = value; }
    public bool IsUnlocked { get => isUnlocked; set => isUnlocked = value; }
    public Recipe Recipe { get => recipe; set => recipe = value; }
}

public class InventoryCraftingController : MonoBehaviour
{

    public void OnTurnInventoryCrafting()
    {
        var unlockedRecipes = new List<Recipe>();

        foreach (var item in InventoryController.Instance.GetPlayerData.Recipes)
        {
            if (item.IsUnlocked)
            {
                unlockedRecipes.Add(item.Recipe);
            }
        }

        PopUpInventory.Instance.TurnCrafting(unlockedRecipes);
    }
}
