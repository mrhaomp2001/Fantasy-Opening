using GameUtil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimerDelayAction : MonoBehaviour, IPoolObject
{
    [SerializeField] private bool isRunOnAwake;
    [SerializeField] private float timeDelay;
    [SerializeField] private UnityEvent onComplete;
    private Timer timer;

    private void Start()
    {
        if (isRunOnAwake)
        {
            OnObjectSpawnAfter();
        }
    }

    public void OnObjectSpawnAfter()
    {
        if (timer == null)
        {
            timer = Timer.DelayAction(timeDelay, () =>
            {
                onComplete?.Invoke();
            });
        }
        else
        {
            timer.Restart();
        }
    }
}
