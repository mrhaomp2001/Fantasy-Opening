using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpTechnologyUpgrade : PopUpSingleton<PopUpTechnologyUpgrade>
{

    private InventoryTechnologyItem technologyItem;

    [SerializeField] private TextMeshProUGUI textContent;
    [SerializeField] private RectTransform buttonUpgrade;

    public void ShowItem(InventoryTechnologyItem valueItem)
    {
        base.Show();
        buttonUpgrade.gameObject.SetActive(true);

        technologyItem = valueItem;
        string content = "";

        content += LanguageController.Instance.GetString(technologyItem.TechModel.TechName) + "\n";
        content += LanguageController.Instance.GetString(technologyItem.TechModel.TechDescription) + "\n\n";
        content += LanguageController.Instance.GetString("ingredient") + ": \n";

        for (int i = 0; i < technologyItem.CostIngredients.Count; i++)
        {
            var ingredient = technologyItem.CostIngredients[i];
            content += $"- {LanguageController.Instance.GetString(ingredient.ItemInput.ItemName)}: {ingredient.Count}\n";
        }

        if (technologyItem.WitchMedalCost > 0)
        {
            content += $"- {LanguageController.Instance.GetString("witch_medal")}: {technologyItem.WitchMedalCost}\n\n";

        }

        if (technologyItem.TechModel.Level >= 1)
        {
            buttonUpgrade.gameObject.SetActive(false);
            content += $"*<color=green>{LanguageController.Instance.GetString("upgraded")}</color>\n";
        }

        if (technologyItem.Prerequisite != null)
        {
            if (technologyItem.Prerequisite.TechModel.Level <= 0)
            {
                buttonUpgrade.gameObject.SetActive(false);
                content += $"*<color=red>{LanguageController.Instance.GetString("upgrade_prerequisite")}</color>\n";

            }
        }

        textContent.SetText(content);
    }

    public void OnUpgrade()
    {
        bool isUpgradeable = true;

        if (WitchSystemController.Instance.Data.WitchMedal >= technologyItem.WitchMedalCost)
        {
            for (int i = 0; i < technologyItem.CostIngredients.Count; i++)
            {
                var ingredient = technologyItem.CostIngredients[i];

                if (!InventoryController.Instance.GetPlayerData.IsItemEnough(ingredient.ItemInput.Id, ingredient.Count))
                {
                    isUpgradeable = false;
                    break;
                }
            }
        }
        else
        {
            // Not enough Witch Medals
            isUpgradeable = false;
        }

        if (isUpgradeable)
        {
            for (int i = 0; i < technologyItem.CostIngredients.Count; i++)
            {
                var ingredient = technologyItem.CostIngredients[i];

                InventoryController.Instance.Consume(ingredient.ItemInput.Id, ingredient.Count, new Callback()
                {
                    onSuccess = () =>
                    {
                        // Successfully consumed ingredient

                        WitchSystemController.Instance.Data.WitchMedal -= technologyItem.WitchMedalCost;

                        technologyItem.OnUpgrade();
                        WitchTechController.Instance.UpdateViews();

                        PopUpInventory.Instance.UpdateViews();
                        StatController.Instance.UpdateHunger();

                        WitchSystemController.Instance.Save();

                        AudioController.Instance.Play("23_collecting", randomPitch: true, 0.8f, 1.2f);

                        Hide();
                    },
                    onFail = (message) =>
                    {
                        // Failed to consume ingredient
                    },
                    onNext = () =>
                    {
                        // Next action after consumption
                    }
                });
            }

        }
        else
        {
            AudioController.Instance.Play("22_can_not", randomPitch: true, 0.8f, 1.2f);
        }
    }
}
