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
    [SerializeField] private int def;
    [SerializeField] private int hungerConsume;

    [SerializeField] protected Rigidbody2D rb;

    [SerializeField] private string[] audioHurtList;

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
    public int Def { get => def; set => def = value; }
    public Animator Animator { get => animator; set => animator = value; }

    private void Start()
    {
        OnObjectSpawnAfter();
    }

    public void OnObjectSpawnAfter()
    {
        hp = HpMax;
        hitbox.position = transform.position;
        gameObject.SetActive(true);
        canMove = true;
        Initialize();
    }

    public virtual void Despawn()
    {
        gameObject.SetActive(false);
        if (GameController.Instance.MoringWaveEnemy.Contains(this))
        {
            GameController.Instance.MoringWaveEnemy.Remove(this);
        }

        if (GameController.Instance.MoringWaveEnemy != null)
        {
            GameController.Instance.UpdateEnemyHealth();
        }
    }

    public virtual void Initialize() { }

    public virtual void TakeDamage(PlayerBullet playerBulletInput)
    {
        hp -= Mathf.Max(0, playerBulletInput.Damage - def);

        if (animator != null)
        {
            animator.Play("hurt");

            PlayHurtAudio();

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

    protected void PlayHurtAudio()
    {
        if (audioHurtList != null && audioHurtList.Length > 0)
        {
            string audioResult = "";

            audioResult = audioHurtList[UnityEngine.Random.Range(0, audioHurtList.Length)];
            AudioController.Instance.Play(audioResult, randomPitch: true, 0.8f, 1.2f);


        }
    }

    public virtual void OnDie()
    {

        ObjectPooler.Instance.SpawnFromPool(enemyDieParticle, Hitbox.transform.position, Quaternion.identity);

        if (dropId > 0)
        {
            WorldItemController.Instance.SpawnItem(dropId, hitbox.transform.position);
        }

        if (hungerConsume > 0)
        {
            InventoryController.Instance.ConsumeHunger(hungerConsume);
        }

        if (expDrop > 0)
        {
            GameObject expGameObject = ObjectPooler.Instance.SpawnFromPool("exp_drop", Hitbox.transform.position, Quaternion.identity);

            var expDropItem = expGameObject.GetComponent<ExpDropItem>();

            expDropItem.Initialize(expDrop);
        }

        gameObject.SetActive(false);
        Despawn();
    }
}
