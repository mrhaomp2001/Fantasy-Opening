using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private string dimensionName;
    [SerializeField] private Transform destination;
    public void OnPlayerTouch(Collider2D other)
    {
        PopUpTransition.Instance.StartTransition(() =>
        {
            PlayerController.Instance.transform.position = destination.position;

            DimensionController.Instance.ChangeDimension(dimensionName);
        });
    }
}
