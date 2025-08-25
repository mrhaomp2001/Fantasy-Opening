using GameUtil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpDropItem : MonoBehaviour, IPoolObject
{

    [SerializeField] private int expCount;
    //[SerializeField] private SpriteRenderer spriteItem;
    [SerializeField] private float dropSpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D hitbox;
    private Timer timerInitItem;

    public void OnObjectSpawnAfter()
    {
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

    public void Initialize(int expCountValue)
    {
        expCount = expCountValue;
    }

    public void OnPlayerTouch()
    {
        InventoryController.Instance.AddExp(expCount);
        gameObject.SetActive(false);
    }

}
