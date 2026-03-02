using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTechnologyItem : MonoBehaviour
{
    [SerializeField] private Sprite[] bg;
    [SerializeField] private Image imageBg;

    [SerializeField] private TextMeshProUGUI textName;

    [SerializeField] private RectTransform lockBg;

    [SerializeField] private InventoryTechnologyItem prerequisite;

    [Header("Ingredients: ")]
    [SerializeField] private int witchMedalCost;
    [SerializeField] private List<Ingredient> costIngredients;

    [Header("Tech Model: ")]
    [SerializeField] private WitchSystemController.WitchTechModel techModel;

    public WitchSystemController.WitchTechModel TechModel { get => techModel; set => techModel = value; }
    public int WitchMedalCost { get => witchMedalCost; set => witchMedalCost = value; }
    public List<Ingredient> CostIngredients { get => costIngredients; set => costIngredients = value; }
    public InventoryTechnologyItem Prerequisite { get => prerequisite; set => prerequisite = value; }

    public void Onload()
    {
        techModel = WitchSystemController.Instance.Data.WitchTechnologies
            .Where((predicate) =>
            {
                return predicate.Id == techModel.Id;
            })
            .FirstOrDefault();
    }

    public void UpdateViews()
    {
        textName.SetText(LanguageController.Instance.GetString(techModel.TechName));

        imageBg.sprite = bg[0];

        if (techModel.Level >= 1)
        {
            imageBg.sprite = bg[1];

        }

        if (Prerequisite == null)
        {
            lockBg.gameObject.SetActive(false);

            return;
        }

        if (Prerequisite.TechModel.Level >= 1)
        {
            lockBg.gameObject.SetActive(false);

            return;
        }

        if (Prerequisite.techModel.Level <= 0)
        {
            lockBg.gameObject.SetActive(true);

            return;
        }

    }

    public void OnUpgrade()
    {
        WitchSystemController.Instance.Data.WitchTechnologies
            .Where((predicate) =>
            {
                return predicate.Id == techModel.Id;
            })
            .FirstOrDefault().Level = 1;
    }

    public void OnClick()
    {
        PopUpTechnologyUpgrade.Instance.ShowItem(this);
    }
}
