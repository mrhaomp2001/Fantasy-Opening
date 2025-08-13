using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpInventoryCraftingTooltip : PopUp
{
    private static PopUpInventoryCraftingTooltip instance;

    public static PopUpInventoryCraftingTooltip Instance { get => instance; set => instance = value; }
    [Header("Crafting Tooltip: ")]

    [SerializeField] private InventoryCraftingTooltipItem result;
    [SerializeField] private List<InventoryCraftingTooltipItem> ingredientViewItems;

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
        Hide();
    }

    public void ShowAtPosition(Vector2 position, Recipe recipe)
    {
        base.Show();
        container.position = position + new Vector2(5f, 0f);

        foreach (var item in ingredientViewItems)
        {
            item.gameObject.SetActive(false);
        }

        result.UpdateViews(recipe.ItemResult.Sprite, recipe.ItemResult.ItemName, recipe.ResultCount.ToString());

        for (int i = 0; i < recipe.Ingredients.Count; i++)
        {
            Ingredient item = recipe.Ingredients[i];
            string textColor = "<color=green>";

            if (InventoryController.Instance.GetPlayerData.CheckItemCount(item.ItemInput.Id) < item.Count)
            {
                textColor = "<color=red>";
            }

            ingredientViewItems[i].UpdateViews(item.ItemInput.Sprite, item.ItemInput.ItemName, $"{textColor}{InventoryController.Instance.GetPlayerData.CheckItemCount(item.ItemInput.Id)}/{item.Count}</color>");

            ingredientViewItems[i].gameObject.SetActive(true);
        }
    }
}
