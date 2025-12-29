using Newtonsoft.Json;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static GameInputController;

[JsonObject(MemberSerialization.OptIn)]
public class PopUpSetting : PopUp
{
    private static PopUpSetting instance;

    public static PopUpSetting Instance { get => instance; set => instance = value; }

    [Header("Master: ")]
    [SerializeField] private TextMeshProUGUI textMaster;
    [SerializeField] private Slider sliderMaster;

    [Header("Background: ")]
    [SerializeField] private TextMeshProUGUI textBackground;
    [SerializeField] private Slider sliderBackground;

    [Header("Effect: ")]
    [SerializeField] private TextMeshProUGUI textEffect;
    [SerializeField] private Slider sliderEffect;

    [Header("Settings: ")]

    [JsonProperty]
    [SerializeField] private float volumeMaster;
    [JsonProperty]
    [SerializeField] private float volumeBackground;
    [JsonProperty]
    [SerializeField] private float volumeEffect;

    [Header("Input Control: ")]
    [SerializeField] private RectTransform containerInputControl;

    [SerializeField] private GameInputController.InputKey currentSelectInputKey;
    [SerializeField] private List<InputControlKeyItem> inputControlKeys;
    [Header("Select Input Control: ")]
    [SerializeField] private RectTransform containerSelectInputControl;
    [SerializeField] private GameObject selectInputControlPrefab;
    [SerializeField] private RectTransform contentSelectInputControl;


    private const string prefKey = "PopUpSetting";
    public RectTransform Container { get => container; }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(instance);
    }

    public void FromJson(string json)
    {
        JsonConvert.PopulateObject(json, instance);
    }

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
        Load();

        for (int i = 0; i < GameInputController.Instance.Keys.Count; i++)
        {
            GameObject item = Instantiate(selectInputControlPrefab, contentSelectInputControl);

            var target = item.GetComponent<ValueSelectInputControlKeyItem>();

            target.UpdateViews(GameInputController.Instance.Keys[i]);
        }
    }

    public override void Show()
    {
        base.Show();
        OnLoad();
    }

    public override void Hide()
    {
        base.Hide();
        containerInputControl.gameObject.SetActive(false);
        containerSelectInputControl.gameObject.SetActive(false);
        AudioController.Instance.PlayButton();

    }

    public void OnLoad()
    {
        Load();
    }

    public void Load()
    {
        if (!PlayerPrefs.HasKey(prefKey))
        {
            // --- Giá trị mặc định ---
            volumeMaster = 100f;
            volumeBackground = 20f;
            volumeEffect = 80f;

            // Gắn vào slider
            sliderMaster.value = volumeMaster;
            sliderBackground.value = volumeBackground;
            sliderEffect.value = volumeEffect;

            // Set AudioMixer
            ApplyVolumesToMixer();

            // Lưu JSON mặc định
            Save();
            return;
        }

        // --- Có dữ liệu, load JSON ---
        string json = PlayerPrefs.GetString(prefKey);

        Debug.Log($"json: {json}");

        FromJson(json);

        // Gắn vào slider
        sliderMaster.value = volumeMaster;
        sliderBackground.value = volumeBackground;
        sliderEffect.value = volumeEffect;

        // Set AudioMixer
        ApplyVolumesToMixer();
    }

    public void Save()
    {
        PlayerPrefs.SetString(prefKey, ToJson());
        Debug.Log($"json: {ToJson()}");

        PlayerPrefs.Save();
    }

    private void ApplyVolumesToMixer()
    {
        float dBMaster = (volumeMaster / 100f) * 20f - 20f;
        float dBBackground = (volumeBackground / 100f) * 20f - 20f;
        float dBEffect = (volumeEffect / 100f) * 20f - 20f;

        AudioController.Instance.AudioMixerMaster.SetFloat("volume_of_master", dBMaster);
        AudioController.Instance.AudioMixerMaster.SetFloat("volume_of_music", dBBackground);
        AudioController.Instance.AudioMixerMaster.SetFloat("volume_of_effect", dBEffect);

        textMaster.text = volumeMaster.ToString();
        textBackground.text = volumeBackground.ToString();
        textEffect.text = volumeEffect.ToString();

    }


    public void OnValueChangedMasterVolume(float value)
    {
        volumeMaster = value;
        textMaster.text = value.ToString();

        float dB = (value / 100f) * 20f - 20f;
        AudioController.Instance.AudioMixerMaster.SetFloat("volume_of_master", dB);
    }

    public void OnValueChangedBackgroundVolume(float value)
    {
        volumeBackground = value;
        textBackground.text = value.ToString();

        float dB = (value / 100f) * 20f - 20f;
        AudioController.Instance.AudioMixerMaster.SetFloat("volume_of_music", dB);
    }

    public void OnValueChangedEffectVolume(float value)
    {
        volumeEffect = value;
        textEffect.text = value.ToString();

        float dB = (value / 100f) * 20f - 20f;
        AudioController.Instance.AudioMixerMaster.SetFloat("volume_of_effect", dB);
    }

    public void OnPointerUp(BaseEventData eventData)
    {
        Save();
        AudioController.Instance.PlayButton();
    }

    public void ShowInputControl()
    {
        containerInputControl.gameObject.SetActive(true);

        AudioController.Instance.PlayButton();
        UpdateViewsInputControl();
    }

    
    public void SelectLeft()
    {
        ShowSelectInputKey();
        currentSelectInputKey = GameInputController.Instance.Left;
        AudioController.Instance.PlayButton();
    }
    public void SelectRight()
    {
        ShowSelectInputKey();
        currentSelectInputKey = GameInputController.Instance.Right;
        AudioController.Instance.PlayButton();
    }
    public void SelectUp() 
    { 
        ShowSelectInputKey();
        currentSelectInputKey = GameInputController.Instance.Up;
        AudioController.Instance.PlayButton();
    }
    public void SelectDown() 
    { 
        ShowSelectInputKey();
        currentSelectInputKey = GameInputController.Instance.Down;
        AudioController.Instance.PlayButton();
    }
    public void SelectInventory()
    { 
        ShowSelectInputKey();
        currentSelectInputKey = GameInputController.Instance.Inventory;
        AudioController.Instance.PlayButton();
    }
    public void SelectHotkey1() 
    { 
        ShowSelectInputKey();
        currentSelectInputKey = GameInputController.Instance.Hotkey1;
        AudioController.Instance.PlayButton();
    }
    public void SelectHotkey2() 
    { 
        ShowSelectInputKey();
        currentSelectInputKey = GameInputController.Instance.Hotkey2;
        AudioController.Instance.PlayButton();
    }
    public void SelectHotkey3() 
    { 
        ShowSelectInputKey();
        currentSelectInputKey = GameInputController.Instance.Hotkey3;
        AudioController.Instance.PlayButton();
    }
    public void SelectHotkey4() 
    { 
        ShowSelectInputKey();
        currentSelectInputKey = GameInputController.Instance.Hotkey4;
        AudioController.Instance.PlayButton();
    }
    public void SelectHotkey5() 
    { 
        ShowSelectInputKey();
        currentSelectInputKey = GameInputController.Instance.Hotkey5;
        AudioController.Instance.PlayButton();
    }
    public void SelectHotkey6() 
    { 
        ShowSelectInputKey();
        currentSelectInputKey = GameInputController.Instance.Hotkey6;
        AudioController.Instance.PlayButton();
    }
    public void SelectHotkey7() 
    { 
        ShowSelectInputKey();
        currentSelectInputKey = GameInputController.Instance.Hotkey7;
        AudioController.Instance.PlayButton();
    }
    public void SelectHotkey8() 
    { 
        ShowSelectInputKey();
        currentSelectInputKey = GameInputController.Instance.Hotkey8;
        AudioController.Instance.PlayButton();
    }
    public void SelectHotkey9() 
    {
        ShowSelectInputKey();
        currentSelectInputKey = GameInputController.Instance.Hotkey9; 
        AudioController.Instance.PlayButton();
    }

    public void ShowSelectInputKey()
    {
        containerSelectInputControl.gameObject.SetActive(true);
        AudioController.Instance.PlayButton();
    }

    public void OnSelectKey(GameInputController.InputKey valueKey)
    {
        currentSelectInputKey.keyCode = valueKey.keyCode;

        containerSelectInputControl.gameObject.SetActive(false);

        UpdateViewsInputControl();

        GameInputController.Instance.Save();
        AudioController.Instance.PlayButton();
    }

    public string GetLocalizedName(InputKey targetKey)
    {
        foreach (var key in GameInputController.Instance.Keys)
        {
            if (key.keyCode == targetKey.keyCode)
            {
                return LanguageController.Instance.GetString(key.name);
            }
        }

        return LanguageController.Instance.GetString(targetKey.name);
    }

    public void UpdateViewsInputControl()
    {
        inputControlKeys[0].UpdateViews(LanguageController.Instance.GetString(GetLocalizedName(GameInputController.Instance.Left)));
        inputControlKeys[1].UpdateViews(LanguageController.Instance.GetString(GetLocalizedName(GameInputController.Instance.Right)));
        inputControlKeys[2].UpdateViews(LanguageController.Instance.GetString(GetLocalizedName(GameInputController.Instance.Up)));
        inputControlKeys[3].UpdateViews(LanguageController.Instance.GetString(GetLocalizedName(GameInputController.Instance.Down)));
        inputControlKeys[4].UpdateViews(LanguageController.Instance.GetString(GetLocalizedName(GameInputController.Instance.Inventory)));

        inputControlKeys[5].UpdateViews(LanguageController.Instance.GetString(GetLocalizedName(GameInputController.Instance.Hotkey1)));
        inputControlKeys[6].UpdateViews(LanguageController.Instance.GetString(GetLocalizedName(GameInputController.Instance.Hotkey2)));
        inputControlKeys[7].UpdateViews(LanguageController.Instance.GetString(GetLocalizedName(GameInputController.Instance.Hotkey3)));
        inputControlKeys[8].UpdateViews(LanguageController.Instance.GetString(GetLocalizedName(GameInputController.Instance.Hotkey4)));
        inputControlKeys[9].UpdateViews(LanguageController.Instance.GetString(GetLocalizedName(GameInputController.Instance.Hotkey5)));
        inputControlKeys[10].UpdateViews(LanguageController.Instance.GetString(GetLocalizedName(GameInputController.Instance.Hotkey6)));
        inputControlKeys[11].UpdateViews(LanguageController.Instance.GetString(GetLocalizedName(GameInputController.Instance.Hotkey7)));
        inputControlKeys[12].UpdateViews(LanguageController.Instance.GetString(GetLocalizedName(GameInputController.Instance.Hotkey8)));
        inputControlKeys[13].UpdateViews(LanguageController.Instance.GetString(GetLocalizedName(GameInputController.Instance.Hotkey9)));

    }
}
