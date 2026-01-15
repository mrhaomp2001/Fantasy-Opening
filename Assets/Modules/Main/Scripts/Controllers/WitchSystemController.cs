using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class WitchSystemController : Singleton<WitchSystemController>
{
    [JsonObject(MemberSerialization.OptIn), System.Serializable]
    public class WitchDataModel
    {
        [JsonProperty]
        [SerializeField] private int level;
        [JsonProperty]
        [SerializeField] private int witchMedal;

        public int Level { get => level; set => level = value; }
        public int WitchMedal { get => witchMedal; set => witchMedal = value; }

        // To JSON (InvariantCulture)
        public string ToJson()
        {
            var settings = new JsonSerializerSettings
            {
                Culture = CultureInfo.InvariantCulture
            };

            return JsonConvert.SerializeObject(this, settings);
        }

        // From JSON (InvariantCulture)
        public static WitchDataModel FromJson(string json)
        {
            var settings = new JsonSerializerSettings
            {
                Culture = CultureInfo.InvariantCulture
            };

            return JsonConvert.DeserializeObject<WitchDataModel>(json, settings);
        }
    }

    private WitchDataModel data;

    private const string prefKey = nameof(WitchSystemController);

    public WitchDataModel Data { get => data; set => data = value; }

    private void Start()
    {
        Load();
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(prefKey))
        {
            string json = PlayerPrefs.GetString(prefKey);
            data = WitchDataModel.FromJson(json);
        }
        else
        {
            data = new WitchDataModel()
            {
                Level = 0,
                WitchMedal = 0
            };

            Save();
        }
    }

    public void Save()
    {
        if (data == null)
            return;

        string json = data.ToJson();
        PlayerPrefs.SetString(prefKey, json);
        PlayerPrefs.Save();
    }

}
