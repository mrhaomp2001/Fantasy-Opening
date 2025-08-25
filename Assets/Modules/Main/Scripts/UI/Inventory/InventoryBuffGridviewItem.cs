using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryBuffGridviewItem : MonoBehaviour
{
    [SerializeField] private Image imageBuff;

    public void ResetViews()
    {
        gameObject.SetActive(false);
    }

    public void UpdateViews(BuffBase buff)
    {
        gameObject.SetActive(true);

        imageBuff.sprite = buff.SpriteBuff; 
    }
}
