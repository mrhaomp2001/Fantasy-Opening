using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffCardviewItem : MonoBehaviour
{
    [SerializeField] private BuffBase buff;
    [SerializeField] private TextMeshProUGUI textBuffName, textBufDescription;
    [SerializeField] private Image imageBuff;
    public void UpdateViews(BuffBase buffBaseValue)
    {
        buff = buffBaseValue;

        textBuffName.text = buff.BuffName;
        textBufDescription.text = buff.Stats.GetString();
        imageBuff.sprite = buff.SpriteBuff;
    }

    public void OnClick()
    {
        PopUpUpgradeSelector.Instance.OnChoiceBuff(buff);
    }
}
