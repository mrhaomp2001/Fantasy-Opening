using GameUtil;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float moveTime;
    [SerializeField] private float despawnTime;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Transform bullet;
    [SerializeField] private SpriteRenderer imageBullet;
    private Vector3[] path;
    private Timer timerDespawn;

    public float MoveTime { get => moveTime; set => moveTime = value; }
    public float DespawnTime { get => despawnTime; set => despawnTime = value; }

    public void SetBulletColor(Color color)
    {
        imageBullet.color = color;
    }

    public void SetPath(string pathNameGet = "")
    {
        bullet.gameObject.SetActive(false);
        bullet.localPosition = Vector3.zero;

        //for (int i = 0; i < CurvePooler.Instance.Curves.Length; i++)
        //{
        //    if (CurvePooler.Instance.Curves[i].name.Equals(pathNameGet))
        //    {
        //        path = CurvePooler.Instance.Curves[i].path;
        //    }
        //}
    }

    public void StartMove(LeanTweenType easing = LeanTweenType.notUsed)
    {
        InitBullet();

        MoveMethod(easing);
    }

    protected virtual void MoveMethod(LeanTweenType leanEasing)
    {
        LeanTween.moveSplineLocal(bullet.gameObject, path, moveTime)
            .setOrientToPath2d(true)
            .setOnComplete(() =>
            {
                Despawn();
            })
            .setEase(leanEasing);
    }

    private void InitBullet()
    {
        bullet.gameObject.SetActive(true);

        LeanTween.cancel(bullet.gameObject);

        LeanTween.alpha(bullet.gameObject, to: 1, 0.3f)
            .setFrom(0f)
            .setEaseOutSine();
        LeanTween.scale(bullet.gameObject, to: Vector3.one, 0.3f)
            .setFrom(new Vector3(1.3f, 1.3f, 1.3f))
            .setEaseOutSine();
    }

    public void StartTimerDespawn()
    {
        InitBullet();

        if (timerDespawn != null)
        {
            timerDespawn.Restart();
        }
        else
        {
            timerDespawn = Timer.DelayAction(despawnTime, () =>
            {
                Despawn();
            });
        }
    }

    public virtual void Despawn()
    {
        LeanTween.cancel(bullet.gameObject);

        LeanTween.alpha(bullet.gameObject, to: 0, 0.5f).setFrom(1f).setEaseOutSine();

        LeanTween.scale(bullet.gameObject, to: new Vector3(1.2f, 1.2f, 1.2f), 0.2f)
            .setFrom(Vector3.one)
            .setEaseOutSine().
            setOnComplete(() =>
            {
                LeanTween.scale(bullet.gameObject, to: new Vector3(0.5f, 0.5f, 0.5f), 0.3f)
                .setEaseOutSine()
                .setOnComplete(() =>
                {
                    this.gameObject.SetActive(false);
                });
            });


    }
}
