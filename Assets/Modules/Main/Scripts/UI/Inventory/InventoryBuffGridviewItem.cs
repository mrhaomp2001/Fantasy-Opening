using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryBuffGridviewItem : MonoBehaviour
{
    [SerializeField] private RectTransform tooltipPosition;

    [SerializeField] private Image imageBuff;

    private BuffBase buff;

    public void ResetViews()
    {
        gameObject.SetActive(false);
    }

    public void UpdateViews(BuffBase buffValue)
    {
        gameObject.SetActive(true);

        buff = buffValue;

        imageBuff.sprite = buffValue.SpriteBuff; 
    }

    public void OnPointerEnter(BaseEventData baseEventData)
    {
        if (baseEventData is PointerEventData pointerEventData)
        {
            PopUpBuffTooltip.Instance.ShowAtPosition(tooltipPosition.position, buff, new Vector2(0f, 1f));
        }
    }

    public void OnPointerExit(BaseEventData baseEventData)
    {
        if (baseEventData is PointerEventData pointerEventData)
        {
            PopUpBuffTooltip.Instance.Hide();
        }
    }
}
