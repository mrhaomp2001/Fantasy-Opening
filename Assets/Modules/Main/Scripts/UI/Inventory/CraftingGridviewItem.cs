using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingGridviewItem : MonoBehaviour
{
    [SerializeField] private Image imageItemResult;

    private Recipe recipeTarget;

    public void UpdateViews(Recipe ValueRecipe)
    {
        recipeTarget = ValueRecipe;

        imageItemResult.sprite = recipeTarget.ItemResult.Sprite;

        gameObject.SetActive(true);

    }

    public void OnClick()
    {
        bool result = true;

        foreach (var item in recipeTarget.Ingredients)
        {
            if (item.Count > InventoryController.Instance.GetPlayerData.CheckItemCount(item.ItemInput.Id))
            {
                result = false;
            }
        }

        if (result && InventoryController.Instance.Add(recipeTarget.ItemResult.Id, recipeTarget.ResultCount))
        {
            foreach (var item in recipeTarget.Ingredients)
            {
                InventoryController.Instance.Consume(item.ItemInput.Id, item.Count, new Callback
                {
                    onSuccess = () =>
                    {
                        string audioResult = "";

                        string[] audioHurtList =
                        {
                            "16_craft_1",
                        };

                        audioResult = audioHurtList[UnityEngine.Random.Range(0, audioHurtList.Length)];

                        AudioController.Instance.Play(audioResult, randomPitch: true, 0.8f, 1.2f);
                    },
                    onFail = (message) =>
                    {

                    },
                    onNext = () =>
                    {

                    }
                });
            }

            PopUpInventory.Instance.UpdateViews();
        }

        PopUpInventoryCraftingTooltip.Instance.ShowAtPosition(imageItemResult.transform.position, recipeTarget);
    }

    public void ResetItem()
    {
        recipeTarget = null;

        gameObject.SetActive(false);
    }

    public void OnPointerEnter(BaseEventData baseEventData)
    {
        if (baseEventData is PointerEventData pointerEventData)
        {
            PopUpInventoryCraftingTooltip.Instance.ShowAtPosition(imageItemResult.transform.position, recipeTarget);
        }
    }

    public void OnPointerExit(BaseEventData baseEventData)
    {
        if (baseEventData is PointerEventData pointerEventData)
        {
            PopUpInventoryCraftingTooltip.Instance.Hide();
        }
    }
}
