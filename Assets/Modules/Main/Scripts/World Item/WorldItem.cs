using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour, IPoolObject
{
    [SerializeField] private int itemId;
    [SerializeField] private SpriteRenderer spriteItem;
    [SerializeField] private float dropSpeed;
    [SerializeField] private Rigidbody2D rb;

    public int ItemId { get => itemId; set => itemId = value; }

    public void OnObjectSpawnAfter()
    {
    }

    public void Initialize(int valueItemId, Sprite valueSprite)
    {
        itemId = valueItemId;
        spriteItem.sprite = valueSprite;

        rb.AddForce(new Vector2(Random.Range(-dropSpeed, dropSpeed), Random.Range(-dropSpeed, dropSpeed)));
    }


    public void OnPlayerTouch()
    {
        InventoryController.Instance.Add(itemId, 1);
        gameObject.SetActive(false);
    }
}
