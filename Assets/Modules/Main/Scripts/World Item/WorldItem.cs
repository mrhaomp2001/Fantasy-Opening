using GameUtil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour, IPoolObject
{
    [SerializeField] private int itemId;
    [SerializeField] private int count;
    [SerializeField] private SpriteRenderer spriteItem;
    [SerializeField] private float dropSpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D hitbox;

    private Timer timerInitItem;
    public int ItemId { get => itemId; set => itemId = value; }

    public void OnObjectSpawnAfter()
    {
    }

    public void Initialize(int valueItemId, Sprite valueSprite, int countValue = 1)
    {
        itemId = valueItemId;
        spriteItem.sprite = valueSprite;
        count = countValue;

        rb.AddForce(new Vector2(Random.Range(-dropSpeed, dropSpeed), Random.Range(-dropSpeed, dropSpeed)));

        hitbox.enabled = false;
        if (timerInitItem == null)
        {
            timerInitItem = Timer.DelayAction(1, () =>
            {
                hitbox.enabled = true;

            });
        }
        else
        {
            timerInitItem.Restart();
        }
    }


    public void OnPlayerTouch()
    {
        if (InventoryController.Instance.Add(itemId, count))
        {
            gameObject.SetActive(false);
        }

    }
}
