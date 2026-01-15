using Newtonsoft.Json;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[JsonObject(MemberSerialization.OptIn)]
public class GameInputController : MonoBehaviour
{
    private static GameInputController instance;

    [System.Serializable]
    public class InputKey
    {
        public string name;
        public KeyCode keyCode;
    }

    [JsonProperty]
    [SerializeField] private InputKey left = new();
    [JsonProperty]
    [SerializeField] private InputKey right = new();
    [JsonProperty]
    [SerializeField] private InputKey up = new();
    [JsonProperty]
    [SerializeField] private InputKey down = new();
    [JsonProperty]
    [SerializeField] private InputKey inventory = new();
    [JsonProperty]
    [SerializeField] private InputKey hotkey1, hotkey2, hotkey3, hotkey4, hotkey5, hotkey6, hotkey7, hotkey8, hotkey9;

    private List<InputKey> keys;

    private const string prefKey = nameof(GameInputController);
    public static GameInputController Instance { get => instance; set => instance = value; }
    public InputKey Left { get => left; set => left = value; }
    public InputKey Right { get => right; set => right = value; }
    public InputKey Up { get => up; set => up = value; }
    public List<InputKey> Keys { get => keys; set => keys = value; }
    public InputKey Down { get => down; set => down = value; }
    public InputKey Inventory { get => inventory; set => inventory = value; }
    public InputKey Hotkey1 { get => hotkey1; set => hotkey1 = value; }
    public InputKey Hotkey2 { get => hotkey2; set => hotkey2 = value; }
    public InputKey Hotkey3 { get => hotkey3; set => hotkey3 = value; }
    public InputKey Hotkey4 { get => hotkey4; set => hotkey4 = value; }
    public InputKey Hotkey5 { get => hotkey5; set => hotkey5 = value; }
    public InputKey Hotkey6 { get => hotkey6; set => hotkey6 = value; }
    public InputKey Hotkey7 { get => hotkey7; set => hotkey7 = value; }
    public InputKey Hotkey8 { get => hotkey8; set => hotkey8 = value; }
    public InputKey Hotkey9 { get => hotkey9; set => hotkey9 = value; }

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
        instance.LoadOrInitialize();
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void FromJson(string json)
    {
        instance = JsonUtility.FromJson<GameInputController>(json);
    }

    public void LoadOrInitialize()
    {
        InitializeKeys();

        left = new InputKey()
        {
            name = "left",
        };

        right = new InputKey()
        {
            name = "right",
        };

        up = new InputKey()
        {
            name = "up",
        };

        down = new InputKey()
        {
            name = "down",
        };

        inventory = new InputKey()
        {
            name = "inventory",
        };

        hotkey1 = new InputKey()
        {
            name = "hotkey1",
        };

        hotkey2 = new InputKey()
        {
            name = "hotkey2",
        };

        hotkey3 = new InputKey()
        {
            name = "hotkey3",
        };

        hotkey4 = new InputKey()
        {
            name = "hotkey4",
        };

        hotkey5 = new InputKey()
        {
            name = "hotkey5",
        };
        hotkey6 = new InputKey()
        {
            name = "hotkey6",
        };

        hotkey7 = new InputKey()
        {
            name = "hotkey7",
        };

        hotkey8 = new InputKey()
        {
            name = "hotkey8",
        };
        
        hotkey9 = new InputKey()
        {
            name = "hotkey9",
        };

        left.keyCode = KeyCode.A;/*(KeyCode)276*/
        right.keyCode = KeyCode.D;/*(KeyCode)276*/
        up.keyCode = KeyCode.W;/*(KeyCode)276*/
        down.keyCode = KeyCode.S;/*(KeyCode)276*/

        inventory.keyCode = KeyCode.E;

        hotkey1.keyCode = KeyCode.Alpha1;
        hotkey2.keyCode = KeyCode.Alpha2;
        hotkey3.keyCode = KeyCode.Alpha3;
        hotkey4.keyCode = KeyCode.Alpha4;
        hotkey5.keyCode = KeyCode.Alpha5;
        hotkey6.keyCode = KeyCode.Alpha6;
        hotkey7.keyCode = KeyCode.Alpha7;
        hotkey8.keyCode = KeyCode.Alpha8;
        hotkey9.keyCode = KeyCode.Alpha9;

        if (PlayerPrefs.HasKey(prefKey))
        {
            Load();
        }

        PlayerPrefs.SetString(prefKey, ToJson());
        PlayerPrefs.Save();
    }
    void InitializeKeys()
    {
        keys = new List<InputKey>();

        // Add additional keys
        AddSpecialKey(LanguageController.Instance.GetString("up_arrow"), KeyCode.UpArrow);
        AddSpecialKey(LanguageController.Instance.GetString("down_arrow"), KeyCode.DownArrow);
        AddSpecialKey(LanguageController.Instance.GetString("left_arrow"), KeyCode.LeftArrow);
        AddSpecialKey(LanguageController.Instance.GetString("right_arrow"), KeyCode.RightArrow);

        AddSpecialKey(LanguageController.Instance.GetString("backspace"), KeyCode.Backspace);
        AddSpecialKey(LanguageController.Instance.GetString("space"), KeyCode.Space);
        AddSpecialKey(LanguageController.Instance.GetString("left_ctrl"), KeyCode.LeftControl);
        AddSpecialKey(LanguageController.Instance.GetString("left_shift"), KeyCode.LeftShift);
        AddSpecialKey(LanguageController.Instance.GetString("tab"), KeyCode.Tab);

        // Add number keys (Alpha0 - Alpha9)
        for (int i = 0; i <= 9; i++)
        {
            InputKey inputKey = new InputKey
            {
                name = i.ToString(),
                keyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), "Alpha" + i)
            };
            keys.Add(inputKey);
        }

        // Add keys from A to Z
        for (char c = 'A'; c <= 'Z'; c++)
        {
            InputKey inputKey = new InputKey
            {
                name = c.ToString(),
                keyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), c.ToString())
            };
            keys.Add(inputKey);
        }


    }


    void AddSpecialKey(string name, KeyCode keyCode)
    {
        InputKey inputKey = new InputKey
        {
            name = name,
            keyCode = keyCode
        };
        keys.Add(inputKey);
    }
    public void Load()
    {
        //FromJson(PlayerPrefs.GetString(PrefKey));

        JSONNode keyValuePairs = JSON.Parse(PlayerPrefs.GetString(prefKey));

        left.keyCode = (KeyCode)keyValuePairs["left"]["keyCode"].AsInt;
        right.keyCode = (KeyCode)keyValuePairs["right"]["keyCode"].AsInt;
        up.keyCode = (KeyCode)keyValuePairs["up"]["keyCode"].AsInt;
        down.keyCode = (KeyCode)keyValuePairs["down"]["keyCode"].AsInt;
        inventory.keyCode = (KeyCode)keyValuePairs["inventory"]["keyCode"].AsInt;

        hotkey1.keyCode = (KeyCode)keyValuePairs["hotkey1"]["keyCode"].AsInt;
        hotkey2.keyCode = (KeyCode)keyValuePairs["hotkey2"]["keyCode"].AsInt;
        hotkey3.keyCode = (KeyCode)keyValuePairs["hotkey3"]["keyCode"].AsInt;
        hotkey4.keyCode = (KeyCode)keyValuePairs["hotkey4"]["keyCode"].AsInt;
        hotkey5.keyCode = (KeyCode)keyValuePairs["hotkey5"]["keyCode"].AsInt;
        hotkey6.keyCode = (KeyCode)keyValuePairs["hotkey6"]["keyCode"].AsInt;
        hotkey7.keyCode = (KeyCode)keyValuePairs["hotkey7"]["keyCode"].AsInt;
        hotkey8.keyCode = (KeyCode)keyValuePairs["hotkey8"]["keyCode"].AsInt;
        hotkey9.keyCode = (KeyCode)keyValuePairs["hotkey9"]["keyCode"].AsInt;
    }

    public void Save()
    {
        PlayerPrefs.SetString(prefKey, ToJson());
        PlayerPrefs.Save();
    }
}
