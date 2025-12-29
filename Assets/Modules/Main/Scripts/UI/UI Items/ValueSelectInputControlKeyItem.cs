using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ValueSelectInputControlKeyItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textKey;
    [SerializeField] private GameInputController.InputKey inputKey;

    public void UpdateViews(GameInputController.InputKey valueKey)
    {
        inputKey = valueKey;
        textKey.text = valueKey.name;
    }

    public void OnClick()
    {
        PopUpSetting.Instance.OnSelectKey(inputKey);
    }
}
