using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class InputControlKeyItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private TextMeshProUGUI textKey;
    [SerializeField] private UnityEvent onClick;

    public void UpdateViews(string valueKey)
    {
        textKey.text = valueKey;
    }

    public void OnClick()
    {
        onClick?.Invoke();
        AudioController.Instance.PlayButton();
    }
}
