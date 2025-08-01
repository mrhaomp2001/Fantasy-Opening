using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerMonoBehaviour : MonoBehaviour
{
    [System.Serializable]
    public class TriggerContent
    {
        public string tag;
        public UnityEvent<Collider2D> unityEventEnter;
        public UnityEvent<Collider2D> unityEventExit;
    }

    [SerializeField] private List<TriggerContent> triggerContents;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (var triggerContent in triggerContents)
        {
            if (collision.CompareTag(triggerContent.tag))
            {
                triggerContent.unityEventEnter.Invoke(collision);
                return;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        foreach (var triggerContent in triggerContents)
        {
            if (collision.CompareTag(triggerContent.tag))
            {
                triggerContent.unityEventExit.Invoke(collision);
                return;
            }
        }
    }
}
