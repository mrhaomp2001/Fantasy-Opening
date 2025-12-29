using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimatorEventHandler : MonoBehaviour
{
    [SerializeField] private UnityEvent unityEvent;

    public void TriggerEvent()
    {
        unityEvent?.Invoke();
    }
}
