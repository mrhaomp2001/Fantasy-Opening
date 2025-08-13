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

public class InventoryCraftingController : MonoBehaviour
{
    [SerializeField] private List<Recipe> recipes;

    public void OnTurnInventoryCrafting()
    {
        PopUpInventory.Instance.TurnCrafting(recipes);
    }
}
