using Newtonsoft.Json;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

public class WitchSystemController : Singleton<WitchSystemController>
{
    [JsonObject(MemberSerialization.OptIn), System.Serializable]
    public class WitchTechModel
    {
        [JsonProperty]
        [SerializeField] private int id;
        [JsonProperty]
        [SerializeField] private int level;

        [SerializeField] private string techName;
        [SerializeField] private string techDescription;

        public int Id { get => id; set => id = value; }
        public int Level { get => level; set => level = value; }
        public string TechName { get => techName; set => techName = value; }
        public string TechDescription { get => techDescription; set => techDescription = value; }
    }

    [JsonObject(MemberSerialization.OptIn), System.Serializable]
    public class WitchDataModel
    {
        [JsonProperty]
        [SerializeField] private int level;
        [JsonProperty]
        [SerializeField] private int witchMedal;
        [JsonProperty]
        [SerializeField] private List<WitchTechModel> witchTechnologies = new List<WitchTechModel>();

        public int Level { get => level; set => level = value; }
        public int WitchMedal { get => witchMedal; set => witchMedal = value; }
        public List<WitchTechModel> WitchTechnologies { get => witchTechnologies; set => witchTechnologies = value; }

        // To JSON (InvariantCulture)
        public string ToJson()
        {
            var settings = new JsonSerializerSettings
            {
                Culture = CultureInfo.InvariantCulture
            };

            Debug.Log($"WitchDataModel ToJson: {JsonConvert.SerializeObject(this, settings)}");

            return JsonConvert.SerializeObject(this, settings);
        }

        //// From JSON (InvariantCulture)
        //public static WitchDataModel FromJson(string json)
        //{
        //    WitchDataModel result = new WitchDataModel();

        //    JSONNode keyValuePairs = JSONNode.Parse(json);



        //    return result;
        //    //var settings = new JsonSerializerSettings
        //    //{
        //    //    Culture = CultureInfo.InvariantCulture
        //    //};

        //    //return JsonConvert.DeserializeObject<WitchDataModel>(json, settings);
        //}
    }

    [SerializeField] private List<WitchTechModel> defaultWitchTechnologies = new List<WitchTechModel>();

    private WitchDataModel data;


    private const string prefKey = nameof(WitchSystemController);

    public WitchDataModel Data { get => data; set => data = value; }

    private void Start()
    {
        Load();
    }

    public void Load()
    {
        string filePath = Path.Combine(Application.persistentDataPath, prefKey + ".json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);

            JSONNode keyValuePairs = JSONNode.Parse(json);

            data = new WitchDataModel
            {
                WitchMedal = keyValuePairs[nameof(WitchDataModel.WitchMedal).ToCamel()].AsInt,
                Level = keyValuePairs[nameof(WitchDataModel.Level).ToCamel()].AsInt,

                WitchTechnologies = new List<WitchTechModel>(defaultWitchTechnologies)
            };

            for (int i = 0; i < keyValuePairs["witchTechnologies"].Count; i++)
            {
                var techNode = keyValuePairs["witchTechnologies"][i];

                var target = data.WitchTechnologies
                    .Where((predicate) =>
                    {
                        return predicate.Id == techNode["id"].AsInt;
                    })
                    .FirstOrDefault();

                target.Level = techNode["level"].AsInt;
            }
            //Debug.Log($"json: {json}");
        }
        else
        {
            data = new WitchDataModel()
            {
                Level = 0,
                WitchMedal = 0,
                WitchTechnologies = new List<WitchTechModel>(defaultWitchTechnologies)
            };

        }
        
        Save();
    }
    public void Save()
    {
        if (data == null)
            return;

        string json = data.ToJson();

        string filePath = Path.Combine(Application.persistentDataPath, prefKey + ".json");
        File.WriteAllText(filePath, json);
    }

}
