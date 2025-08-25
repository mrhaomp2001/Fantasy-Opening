using Newtonsoft.Json;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[JsonObject(MemberSerialization.OptIn), System.Serializable]
public class ProgressionController : MonoBehaviour
{
    private static ProgressionController instance;

    [SerializeField] private List<ProgressionBase> progressions;

    public List<ProgressionBase> Progressions { get => progressions; set => progressions = value; }
    public static ProgressionController Instance { get => instance; set => instance = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Save()
    {
        for (int i = 0; i < progressions.Count; i++)
        {
            ProgressionBase item = progressions[i];
            item.OnSave();
        }
        InventoryController.Instance.GetPlayerData.Progressions = progressions;
    }

    public void LoadData(JSONNode keyValuePairsValue)
    {
        for (int i = 0; i < keyValuePairsValue.Count; i++)
        {
            var progressionData = keyValuePairsValue[i];

            var targetProgression = progressions
                .Where(predicate =>
                {
                    return predicate.ProgressionName.Equals(progressionData["progressionName"]);
                })
                .FirstOrDefault();

            if (targetProgression != null)
            {
                targetProgression.IsReady = progressionData["isReady"].AsBool;
                targetProgression.IsActivated = progressionData["isActivated"].AsBool;
                targetProgression.IsCompleted = progressionData["isCompleted"].AsBool;
                targetProgression.IsSaved = progressionData["isSaved"].AsBool;
            }
        }

    }

    public void Load()
    {
        for (int i = 0; i < progressions.Count; i++)
        {
            var progression = progressions[i];

            progression.OnLoad();
        }
    }
}
