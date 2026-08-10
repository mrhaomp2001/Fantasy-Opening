using Newtonsoft.Json;
using SimpleJSON;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

[JsonObject(MemberSerialization.OptIn)]
public class LanguageController : MonoBehaviour
{
    private static LanguageController instance;

    [JsonProperty]
    [SerializeField] private string language;
    private const string defaultLanguage = "en";

    private JSONNode languageContent;
    private const string prefKey = nameof(LanguageController);

    private Dictionary<string, TextAsset> languageFiles = new();
    public static LanguageController Instance { get => instance; private set => instance = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadLanguageFiles();
        Load();
    }

    private void LoadLanguageFiles()
    {
        TextAsset[] files = Resources.LoadAll<TextAsset>("Localization");

        foreach (TextAsset file in files)
        {
            string languageCode = file.name.ToLower();

            languageFiles[languageCode] = file;
        }
    }

    public void Save()
    {
        PlayerPrefs.SetString(prefKey, ToJson());

        PlayerPrefs.Save();
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(prefKey))
        {
            string json = PlayerPrefs.GetString(prefKey);
            FromJson(json);
        }
        else
        {
            // Chưa từng chọn ngôn ngữ
            language = defaultLanguage;
            LoadLanguageContent();
        }

        Debug.Log($"LanguageController loaded: {language} - {languageContent["lang_name"]}");
    }

    private void LoadLanguageContent()
    {
        languageContent = null;

        if (languageFiles.TryGetValue(language.ToLower(), out TextAsset file))
        {
            languageContent = JSONNode.Parse(file.text);
        }

        // Fallback về English nếu language không tồn tại
        if (languageContent == null &&
            languageFiles.TryGetValue(defaultLanguage, out TextAsset defaultFile))
        {
            languageContent = JSONNode.Parse(defaultFile.text);
        }
    }

    public void SetLanguage(string languageCode)
    {
        if (string.IsNullOrEmpty(languageCode))
            return;

        languageCode = languageCode.ToLower();

        // Kiểm tra ngôn ngữ có tồn tại
        if (!languageFiles.ContainsKey(languageCode))
        {
            Debug.LogWarning(
                $"Language '{languageCode}' không tồn tại."
            );

            return; 
        }

        // Cập nhật ngôn ngữ hiện tại
        language = languageCode;

        // Load nội dung ngôn ngữ mới
        LoadLanguageContent();

        // Lưu ngôn ngữ
        Save();

        // Cập nhật tất cả LanguageSetter
        LanguageSetter[] setters = FindObjectsByType<LanguageSetter>(FindObjectsInactive.Include);

        foreach (LanguageSetter setter in setters)
        {
            setter.SetLanguage();
        }
    }



    public string ToJson()
    {
        return JsonConvert.SerializeObject(instance);
    }

    public void FromJson(string json)
    {
        var settings = new JsonSerializerSettings
        {
            Culture = CultureInfo.InvariantCulture
        };
        JsonConvert.PopulateObject(json, instance, settings);
    }
    public static string ProcessTags(string value)
    {
        string pattern = @"\[(\w+)\](.*?)\[/\1\]";

        return Regex.Replace(value, pattern, match =>
        {
            string tag = match.Groups[1].Value;
            string content = match.Groups[2].Value;

            return tag switch
            {
                "itemName" => ItemName(content),
                "myTag2" => HandleMyTag2(content),
                _ => match.Value,
            };
        });
    }

    static string ItemName(string content)
    {
        var target = ItemDatabase.Instance.Items.FirstOrDefault((predicate) =>
        {
            return predicate.Id.ToString().Equals(content);
        });

        return $"{target.ItemName}";
    }

    static string HandleMyTag2(string content)
    {
        return $"something2";
    }

    public void UpdateLanguageContent()
    {
    }

    public string GetString(string key)
    {
        string result = key;

        if (languageContent[key] != null && languageContent[key].Value != "")
        {
            result = languageContent[key].Value;
        }

        string resultFinal = ProcessTags(result);

        return resultFinal;
    }

    #region CompareLanguageKeys

    //#if UNITY_EDITOR

    //    [ContextMenu("Compare VI & EN Keys")]
    //    public void CompareLanguageKeys()
    //    {
    //        JSONNode viJson = JSON.Parse(vi.text);
    //        JSONNode enJson = JSON.Parse(en.text);

    //        HashSet<string> viKeys = new();
    //        HashSet<string> enKeys = new();

    //        foreach (KeyValuePair<string, JSONNode> pair in viJson)
    //            viKeys.Add(pair.Key);

    //        foreach (KeyValuePair<string, JSONNode> pair in enJson)
    //            enKeys.Add(pair.Key);

    //        Debug.Log($"VI: {viKeys.Count} keys");
    //        Debug.Log($"EN: {enKeys.Count} keys");

    //        bool hasError = false;

    //        // Key có trong EN nhưng thiếu ở VI
    //        foreach (string key in enKeys)
    //        {
    //            if (!viKeys.Contains(key))
    //            {
    //                Debug.LogError($"[Missing in VI] {key}");
    //                hasError = true;
    //            }
    //        }

    //        // Key có trong VI nhưng không có ở EN
    //        foreach (string key in viKeys)
    //        {
    //            if (!enKeys.Contains(key))
    //            {
    //                Debug.LogError($"[Extra in VI] {key}");
    //                hasError = true;
    //            }
    //        }

    //        if (!hasError)
    //        {
    //            Debug.Log("<color=green>✔ VI và EN có cùng bộ key.</color>");
    //        }
    //    }

    //#endif
    #endregion

}

