﻿using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

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

    private List<InputKey> keys;

    private const string prefKey = "GameInputController";
    public static GameInputController Instance { get => instance; set => instance = value; }
    public InputKey Left { get => left; set => left = value; }
    public InputKey Right { get => right; set => right = value; }
    public InputKey Up { get => up; set => up = value; }
    public List<InputKey> Keys { get => keys; set => keys = value; }
    public InputKey Down { get => down; set => down = value; }
    public InputKey Inventory { get => inventory; set => inventory = value; }

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

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

        if (PlayerPrefs.HasKey(prefKey))
        {
            Load();
        }
        else
        {
            left.keyCode = KeyCode.A;/*(KeyCode)276*/
            right.keyCode = KeyCode.D;/*(KeyCode)276*/
            up.keyCode = KeyCode.W;/*(KeyCode)276*/
            down.keyCode = KeyCode.S;/*(KeyCode)276*/

            inventory.keyCode = KeyCode.E;

            PlayerPrefs.SetString(prefKey, ToJson());
            PlayerPrefs.Save();
        }
    }
    void InitializeKeys()
    {
        keys = new List<InputKey>();

        // Add additional keys

        // TODO: Language here!
        AddSpecialKey("Mũi tên lên", KeyCode.UpArrow);
        AddSpecialKey("Mũi tên xuống", KeyCode.DownArrow);
        AddSpecialKey("Mũi tên trái", KeyCode.LeftArrow);
        AddSpecialKey("Mũi tên phải", KeyCode.RightArrow);
        AddSpecialKey("Nút xóa (Backspace)", KeyCode.Backspace);
        AddSpecialKey("Nút cách (Space)", KeyCode.Space);
        AddSpecialKey("Ctrl trái", KeyCode.LeftControl);
        AddSpecialKey("Shift trái", KeyCode.LeftShift);
        AddSpecialKey("Phím tab", KeyCode.Tab);

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
    }

    public void Save()
    {
        PlayerPrefs.SetString(prefKey, ToJson());
        PlayerPrefs.Save();
    }
}
