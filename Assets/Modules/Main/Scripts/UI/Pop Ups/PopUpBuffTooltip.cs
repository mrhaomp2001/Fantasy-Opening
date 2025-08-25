using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpBuffTooltip : PopUp
{
    private static PopUpBuffTooltip instance;

    private BuffBase buff;
    [Header("Inventory Tooltip: ")]

    [SerializeField] private TextMeshProUGUI textItemName;
    [SerializeField] private TextMeshProUGUI textItemDescription;
    [SerializeField] private TextMeshProUGUI textItemPrice;
    [SerializeField] private Image imageItemSprite;

    public static PopUpBuffTooltip Instance { get => instance; set => instance = value; }
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

    public void ShowAtPosition(Vector2 position, BuffBase buffValue, Vector2 pivot)
    {
        if (buffValue != null)
        {
            base.Show();
            container.position = position + new Vector2(5f, 0f);
            buff = buffValue;

            textItemName.text = buff.BuffName;
            textItemDescription.text = buff.BuffDescription;
            textItemPrice.text = buff.Stats.GetString();
            imageItemSprite.sprite = buff.SpriteBuff;

            container.pivot = pivot;
        }

    }
}

