using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IPoolObject
{
    [SerializeField] private int hp;
    [SerializeField] private int hpMax;
    [SerializeField] private bool canMove;
    [SerializeField] private string enemyDieParticle;
    [SerializeField] protected Transform hitbox;
    [SerializeField] protected Transform sprite;
    [SerializeField] private Animator animator;
    [SerializeField] private int touchDamage;

    [SerializeField] protected Rigidbody2D rb;
    [Header("Drop: ")]
    [SerializeField] private int dropId;
    [SerializeField] private int expDrop;
    public int Hp { get => hp; set => hp = value; }
    public Transform Hitbox { get => hitbox; set => hitbox = value; }
    public int TouchDamage
    {
        get => touchDamage;

        set
        {
            touchDamage = value;
        }
    }
    public bool CanMove
    {
        get => canMove; set
        {
            if (!value)
            {
                rb.velocity = Vector2.zero;
            }
            canMove = value;
        }
    }

    public Rigidbody2D Rb { get => rb; set => rb = value; }
    public int HpMax { get => hpMax; set => hpMax = value; }

    private void Start()
    {
        OnObjectSpawnAfter();
    }

    public void OnObjectSpawnAfter()
    {
        hp = HpMax;
        hitbox.position = transform.position;
        gameObject.SetActive(true);

        Initialize();
    }

    public virtual void Despawn()
    {
        if (GameController.Instance.MoringWaveEnemy.Contains(this))
        {
            GameController.Instance.MoringWaveEnemy.Remove(this);
        }
        gameObject.SetActive(false);

        if (GameController.Instance.MoringWaveEnemy != null)
        {
            GameController.Instance.UpdateEnemyHealth();
        }
    }

    public virtual void Initialize() { }

    public virtual void TakeDamage(PlayerBullet playerBulletInput)
    {
        hp--;

        if (animator != null)
        {
            animator.Play("hurt");
        }

        if (hp <= 0)
        {
            OnDie();
        }

        if (GameController.Instance.MoringWaveEnemy != null)
        {
            GameController.Instance.UpdateEnemyHealth();
        }
    }

    public virtual void OnDie()
    {

        ObjectPooler.Instance.SpawnFromPool(enemyDieParticle, Hitbox.transform.position, Quaternion.identity);

        if (dropId > 0)
        {
            WorldItemController.Instance.SpawnItem(dropId, hitbox.transform.position);
        }

        if (expDrop > 0)
        {
            GameObject expGameObject = ObjectPooler.Instance.SpawnFromPool("exp_drop", Hitbox.transform.position, Quaternion.identity);

            var expDropItem = expGameObject.GetComponent<ExpDropItem>();

            expDropItem.Initialize(expDrop);
        }

        if (GameController.Instance.MoringWaveEnemy.Contains(this))
        {
            GameController.Instance.MoringWaveEnemy.Remove(this);
        }

        gameObject.SetActive(false);
    }
}
