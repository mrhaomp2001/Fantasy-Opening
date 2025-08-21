using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, JsonObject(MemberSerialization.OptIn)]
public class ProgressionBase : MonoBehaviour
{
    [JsonProperty]
    [SerializeField] private string progressionName;
    [JsonProperty]
    [SerializeField] private bool isReady;
    [JsonProperty]
    [SerializeField] private bool isActivated;
    [JsonProperty]
    [SerializeField] private bool isCompleted;
    [JsonProperty]
    [SerializeField] private bool isSaved;

    public string ProgressionName { get => progressionName; set => progressionName = value; }
    public bool IsReady { get => isReady; set => isReady = value; }
    public bool IsActivated { get => isActivated; set => isActivated = value; }
    public bool IsCompleted { get => isCompleted; set => isCompleted = value; }
    public bool IsSaved { get => isSaved; set => isSaved = value; }

    public virtual void OnReady()
    {
        IsReady = true;
    }

    public virtual void OnActived()
    {
        if (!isSaved)
        {
            IsActivated = true;
        }
    }

    public virtual void OnCompleted()
    {
        if (isActivated)
        {
            isCompleted = true;
        }
    }

    public virtual void OnSave()
    {
        if (isCompleted)
        {
            isSaved = true;
        }
    }

    public virtual void OnLoad()
    {

    }
}
