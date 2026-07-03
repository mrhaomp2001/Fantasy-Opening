using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingGridviewItem : MonoBehaviour
{
    [SerializeField] private Image imageItemResult;

    private Recipe recipeTarget;

    [SerializeField] private List<RectTransform> qualityHolders;

    private void UpdateQuality()
    {
        foreach (var qualityHolder in qualityHolders)
        {
            qualityHolder.gameObject.SetActive(false);
        }

        switch (recipeTarget.ItemResult.Quality)
        {
            case ItemQuality.Normal:
                qualityHolders[0].gameObject.SetActive(true);
                break;
            case ItemQuality.Common:
                qualityHolders[1].gameObject.SetActive(true);
                break;
            case ItemQuality.Rare:
                qualityHolders[2].gameObject.SetActive(true);
                break;
            case ItemQuality.SuperRare:
                qualityHolders[3].gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }


    public void UpdateViews(Recipe ValueRecipe)
    {
        recipeTarget = ValueRecipe;

        imageItemResult.sprite = recipeTarget.ItemResult.Sprite;

        gameObject.SetActive(true);

        UpdateQuality();
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

        if (recipeTarget is BuildingWithCraftingAndTime.RecipeWithTime target)
        {
            string audioResult = "";

            string[] audioHurtList =
            {
                "16_craft_1",
            };

            audioResult = audioHurtList[UnityEngine.Random.Range(0, audioHurtList.Length)];

            AudioController.Instance.Play(audioResult, minPithch: 0.8f, maxPitch: 1.2f);

            PopUpInventory.Instance.CraftingTimeStationCurrent.StartCrafting(target);

            PopUpInventory.Instance.Hide();

            return;
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

                        AudioController.Instance.Play(audioResult, minPithch: 0.8f, maxPitch: 1.2f);
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
