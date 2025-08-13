using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LanguageSetter : MonoBehaviour
{
    private void Start()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();

        if (text == null)
        {
            return;
        }
        var targetString = LanguageController.Instance.GetString(text.text);

        text.text = targetString;
    }
}
