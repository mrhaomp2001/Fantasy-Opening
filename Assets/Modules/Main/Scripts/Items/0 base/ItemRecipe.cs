using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "Farm/Items/Recipe")]
public class ItemRecipe : ItemBase
{
    [SerializeField] private int idRecipe;

    public int IdRecipe { get => idRecipe; set => idRecipe = value; }

    public void UnlockRecipe()
    {
        InventoryController.Instance.Consume(Id, 1, new Callback
        {
            onSuccess = () =>
            {
                var recipe = InventoryController.Instance.GetPlayerData.Recipes
                    .Where(predicate => predicate.Id == idRecipe)
                    .FirstOrDefault();
                if (recipe != null)
                {
                    recipe.IsUnlocked = true;
                }

            },
            onFail = (message) =>
            {

            },
            onNext = () =>
            {

            }
        });

    }
}
