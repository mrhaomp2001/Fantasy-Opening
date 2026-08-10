using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LanguageSetter : MonoBehaviour
{
    private string key;
    private TextMeshProUGUI text;
    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();

        key = text.text;

        bool flowControl = SetLanguage();
        if (!flowControl)
        {
            return;
        }
    }

    public bool SetLanguage()
    {

        if (text == null)
        {
            return false;
        }

        var targetString = LanguageController.Instance.GetString(key);

        text.SetText(targetString);
        return true;
    }
}
