using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderInLayerSetter : MonoBehaviour
{

    private void OnEnable()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = (int)-(transform.position.y * 100f);
    }
}
