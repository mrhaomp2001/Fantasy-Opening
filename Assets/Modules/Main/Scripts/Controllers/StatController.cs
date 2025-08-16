using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatController : MonoBehaviour
{
    private static StatController instance;
    [Header("HP: ")]
    [SerializeField] private Slider sliderHp;
    [SerializeField] private TextMeshProUGUI textHp;

    public static StatController Instance { get => instance; set => instance = value; }

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
        sliderHp.minValue = 0;

        UpdateViews();
    }

    public void UpdateViews()
    {
        UpdateHp();
    }

    public void UpdateHp()
    {
        sliderHp.maxValue = InventoryController.Instance.GetPlayerData.HpMax;
        sliderHp.value = InventoryController.Instance.GetPlayerData.Hp;

        textHp.text = $"{sliderHp.value}/{sliderHp.maxValue}";
    }
}
