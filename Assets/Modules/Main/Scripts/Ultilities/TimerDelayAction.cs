using GameUtil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimerDelayAction : MonoBehaviour, IPoolObject
{
    [SerializeField] private float timeDelay;
    [SerializeField] private UnityEvent onComplete;
    private Timer timer;
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
