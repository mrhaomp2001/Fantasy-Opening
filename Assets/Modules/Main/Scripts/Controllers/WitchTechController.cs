using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WitchTechController : Singleton<WitchTechController>
{
    [SerializeField] private TextMeshProUGUI textTechnology;
    [JsonProperty]
    [SerializeField] private List<InventoryTechnologyItem> technologyItems;


    public List<InventoryTechnologyItem> TechnologyItems { get => technologyItems; set => technologyItems = value; }
    private void Start()
    {
        OnLoad();
    }

    public void OnLoad()
    {
        foreach (var item in technologyItems)
        {
            item.Onload();
        }
    }

    public void UpdateViews()
    {
        foreach (var item in technologyItems)
        {
            item.UpdateViews();
        }

        textTechnology.SetText(LanguageController.Instance.GetString("technology_description"), WitchSystemController.Instance.Data.Level, WitchSystemController.Instance.Data.WitchMedal);

    }
}
