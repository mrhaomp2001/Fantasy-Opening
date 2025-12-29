using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryBuffGridviewItem : MonoBehaviour
{
    [SerializeField] private Sprite mask;
    [SerializeField] private RectTransform tooltipPosition;

    [SerializeField] private Image imageBuff;

    private BuffBase buff;

    public void ResetViews()
    {
        buff = null;
        Render();
    }

    public void UpdateViews(BuffBase buffValue)
    {
        buff = buffValue;
        Render();
    }

    public void Render()
    {
        if (buff != null)
        {
            imageBuff.sprite = buff.SpriteBuff;
        }
        else
        {
            imageBuff.sprite = mask;
        }
    }

    public void OnPointerEnter(BaseEventData baseEventData)
    {
        if (buff == null)
        {
            return;
        }

        if (baseEventData is PointerEventData pointerEventData)
        {
            PopUpBuffTooltip.Instance.ShowAtPosition(tooltipPosition.position, buff, new Vector2(0f, 1f));
        }
    }

    public void OnPointerExit(BaseEventData baseEventData)
    {
        if (buff == null)
        {
            return;
        }

        if (baseEventData is PointerEventData pointerEventData)
        {
            PopUpBuffTooltip.Instance.Hide();
        }
    }
}
