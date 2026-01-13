using Proyecto26;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpChangelog : PopUp
{
    private static PopUpChangelog instance;

    public static PopUpChangelog Instance { get => instance; set => instance = value; }

    [SerializeField] private string result;
    [SerializeField] private string targetEndpoint = "https://raw.githubusercontent.com/mrhaomp2001/mrhaomp2001/refs/heads/main/configs/fantasy_opening.json";

    private string checkStatus;

    [SerializeField] private TextMeshProUGUI textContent;

    private JSONNode jsonDataResponse;

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
        SendGetRequest();
        Show();
    }

    private void SendGetRequest()
    {
        jsonDataResponse = JSONNode.Parse("{\"version\":\"N/A\",\"changelog\":\"N/A\"}");
        if (PlayerPrefs.HasKey("PopUpChangelog"))
        {
            jsonDataResponse = JSONNode.Parse(PlayerPrefs.GetString("PopUpChangelog"));
        }

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            checkStatus = $"<color=#FF0000>{LanguageController.Instance.GetString("no_internet")}</color>";
            UpdateViews();

            return;
        }

        checkStatus = $"<color=#FFFF00>{LanguageController.Instance.GetString("checking_version")}...</color>";
        UpdateViews();

        var headers = new Dictionary<string, string>
        {
            { "Content-Type", "application/json" },
            { "Accept", "application/json" }
            // Nếu cần token:
            // { "Authorization", "Bearer YOUR_TOKEN" }
        };

        RestClient.Request(new RequestHelper
        {
            Uri = targetEndpoint,
            Method = "GET",
            Headers = headers,
            Timeout = 10,
            EnableDebug = true
        })
        .Then(response =>
        {
            Save(response.Text);

            if (instance != null)
            {
                // response.Text là string trả về
                Debug.Log($"GET SUCCESS: {response.Text}");

                jsonDataResponse = JSONNode.Parse(response.Text);

                checkStatus = $"<color=#00FF00>{LanguageController.Instance.GetString("check_success")}</color>";
                UpdateViews();
            }
        })
        .Catch(error =>
        {
            if (instance != null)
            {
                Debug.Log($"GET ERROR: {error}");

                checkStatus = $"<color=#FF0000>{LanguageController.Instance.GetString("unknown_error")}</color>";
                UpdateViews();
            }

        }).Finally(() =>
        {

        });
    }

    public override void Show()
    {
        base.Show();
        UpdateViews();
    }

    public override void Hide()
    {
        base.Hide();
        AudioController.Instance.PlayButton();
    }

    public void Save(string responseText)
    {
        PlayerPrefs.SetString("PopUpChangelog", responseText);
        PlayerPrefs.Save();

        //Debug.Log("Saved PopUpChangelog:");
        //Debug.Log(responseText);
    }

    public void UpdateViews()
    {
        if (instance != null && textContent != null)
        {
            textContent.SetText(
                $"{LanguageController.Instance.GetString("check_status")}: {checkStatus} \n" +
                $"{LanguageController.Instance.GetString("current_version")}: {Application.version} \n" +
                $"{LanguageController.Instance.GetString("latest_version")}: {jsonDataResponse["version"].Value} \n" +
                $"--\n" +
                $"{jsonDataResponse["changelog"].Value}\n" +
                $""
                );
        }
    }

    public void OpenPlayStore()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.MysticalDreamers.FantasyOpening");
    }


    public void OpenWebsite()
    {
        Application.OpenURL("https://mystical-dreamers.itch.io/fantasy-opening");
    }
}
