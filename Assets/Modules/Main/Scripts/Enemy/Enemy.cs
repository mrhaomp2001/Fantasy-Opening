using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IPoolObject
{
    [SerializeField] private int hp;
    [SerializeField] private int hpMax;
    [SerializeField] private string enemyDieParticle;
    [SerializeField] protected Transform hitbox;
    [SerializeField] protected Transform sprite;
    [SerializeField] private Animator animator;
    [SerializeField] private int touchDamage;
    [Header("Drop: ")]
    [SerializeField] private int dropId;
    public int Hp { get => hp; set => hp = value; }
    public Transform Hitbox { get => hitbox; set => hitbox = value; }
    public int TouchDamage { get => touchDamage; set => touchDamage = value; }

    private void Start()
    {
        OnObjectSpawnAfter();
    }

    public void OnObjectSpawnAfter()
    {
        hp = hpMax;
        hitbox.position = transform.position;
        gameObject.SetActive(true);

        Initialize();
    }

    public virtual void Despawn()
    {
        gameObject.SetActive(false);
    }

    public virtual void Initialize(){}

    public virtual void TakeDamage(PlayerBullet playerBulletInput)
    {
        hp--;

        if (animator != null)
        {
            animator.Play("hurt");
        }

        if (hp <= 0)
        {
            if (dropId > 0)
            {
                WorldItemController.Instance.SpawnItem(dropId, hitbox.transform.position);
            }

            gameObject.SetActive(false);

            ObjectPooler.Instance.SpawnFromPool(enemyDieParticle, Hitbox.transform.position, Quaternion.identity);

        }
    }
}
