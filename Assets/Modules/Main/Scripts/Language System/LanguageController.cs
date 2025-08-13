using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
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
    }

    private void Start()
    {
        languageContent = JSONNode.Parse(vn.text);
    }

    public string GetString(string key)
    {
        string result = key;

        if (languageContent[key] != null && languageContent[key].Value != "")
        {
            result = languageContent[key].Value;
        }

        return result;
    }
}
