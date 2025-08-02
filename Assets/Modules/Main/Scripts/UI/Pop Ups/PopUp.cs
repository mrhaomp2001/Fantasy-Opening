using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    [SerializeField] protected RectTransform container;

    public virtual void Show()
    {
        container.gameObject.SetActive(true);

    }

    public virtual void Hide()
    {
        container.gameObject.SetActive(false);
    }

    public virtual void Turn()
    {
        container.gameObject.SetActive(!container.gameObject.activeSelf);
    }
}
