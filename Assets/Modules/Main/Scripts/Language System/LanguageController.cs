using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class LanguageController : MonoBehaviour
{
    private static LanguageController instance;
    [SerializeField] private TextAsset vn;

    private JSONNode languageContent;

    public static LanguageController Instance { get => instance; private set => instance = value; }

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
        languageContent = JSONNode.Parse(vn.text);
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
        var target = ItemDatabase.Instance.Items.Where((predicate) =>
        {
            return predicate.Id.ToString().Equals(content);
        }).FirstOrDefault();

        return $"{target.ItemName}";
    }

    static string HandleMyTag2(string content)
    {
        return $"something2";
    }

    public string GetString(string key)
    {

        if (languageContent == null)
        {
            languageContent = JSONNode.Parse(vn.text);
        }

        string result = key;

        if (languageContent[key] != null && languageContent[key].Value != "")
        {
            result = languageContent[key].Value;
        }

        string resultFinal = ProcessTags(result);

        return resultFinal;
    }
}
