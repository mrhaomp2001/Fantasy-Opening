using Proyecto26;
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// This library provides a way to download files. <br />
/// Enter a URL and call <see cref="RemoteFileDownloader.DownloadFile(string)"/>, and the download process will run automatically. <br />
/// Files will be cached to Application.persistentDataPath. <br />
/// <br />
/// This library provides: <br />
/// - File downloading via paths defined by JSON content <br />
/// - Supports file caching <br />
/// - Automatic cache deletion when downloading new files <br />
/// - Automatic downloading when a new version update is available <br />
/// - Automatic downloading when a link difference is detected compared to the cached version <br />
/// - File type categorization for easier handling <br />
/// - Offline file loading support (load from cache) <br />
///  </summary>
///  <br />
///  
/// Response JSON Model: <br />
/// } <br />
///    "id": "1", <br />
///    "type": "1", <br />
///    "version": "1", <br />
///    "link": "download the file from this link" <br />
///  } <br />
/// 

public class RemoteFileDownloader : MonoBehaviour
{
    private const string historyPath = "downloaded_history.json";

    private List<JSONNode> downloadHistory = new List<JSONNode>();
    private string HistoryFullPath => Path.Combine(Application.persistentDataPath, historyPath);

    private void Start()
    {
        LoadDownloadHistory();
    }

    // DOWNLOAD HISTORY

    private void LoadDownloadHistory()
    {
        downloadHistory.Clear();

        if (!File.Exists(HistoryFullPath))
            return;

        string jsonText = File.ReadAllText(HistoryFullPath);
        JSONArray array = JSON.Parse(jsonText).AsArray;

        foreach (JSONNode node in array)
        {
            downloadHistory.Add(node);
        }
    }

    private void SaveDownloadHistory()
    {
        JSONArray array = new JSONArray();

        foreach (var node in downloadHistory)
        {
            array.Add(node);
        }

        File.WriteAllText(HistoryFullPath, array.ToString());
    }

    // MAIN FLOW

    public string TryLoadFromLocalCache(string link)
    {
        foreach (var cached in downloadHistory)
        {
            if (cached["link"] == link)
            {
                string fileName = Path.GetFileName(link);
                string filePath = Path.Combine(Application.persistentDataPath, fileName);

                if (File.Exists(filePath))
                {
                    return filePath;
                }

            }
        }

        return string.Empty;
    }

    public void DownloadFile(string url)
    {
        // Check internet connectivity and download from local cache if offline
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log(TryLoadFromLocalCache(url));
        }
        else
        {
            RestClient.Get(url).Then(res =>
            {
                JSONNode json = JSON.Parse(res.Text);

                string id = json["id"];
                string type = json["type"];
                string version = json["version"];
                string link = json["link"];

                string newFileName = Path.GetFileName(link);
                string newFilePath = Path.Combine(Application.persistentDataPath, newFileName);

                // this is cached item
                JSONNode cachedItem = null;

                foreach (var cached in downloadHistory)
                {
                    if (cached["id"] == id && cached["type"] == type)
                    {
                        cachedItem = cached;
                        break;
                    }
                }

                bool needDownload = true;

                if (cachedItem != null)
                {
                    string cachedVersion = cachedItem["version"];
                    string cachedLink = cachedItem["link"];
                    string oldFileName = Path.GetFileName(cachedLink);
                    string oldFilePath = Path.Combine(Application.persistentDataPath, oldFileName);

                    // Check if we can use the cached file
                    if (cachedVersion == version && File.Exists(oldFilePath) && newFileName == oldFileName)
                    {
                        needDownload = false;
                        Debug.Log(oldFilePath);
                    }
                    else if (File.Exists(oldFilePath))
                    {
                        File.Delete(oldFilePath);
                    }
                }

                // Download the new file if needed
                if (needDownload)
                {
                    RestClient.Get(link).Then(resNewFile =>
                    {
                        File.WriteAllBytes(newFilePath, resNewFile.Data);

                        UpdateDownloadHistory(json);
                        SaveDownloadHistory();

                        Debug.Log(newFilePath);
                    })
                    .Catch(err => Debug.LogError(err));
                }
            })
            .Catch(err => Debug.LogError(err));
        }
    }

    private void UpdateDownloadHistory(JSONNode metadata)
    {
        string id = metadata["id"];
        string type = metadata["type"];

        for (int i = 0; i < downloadHistory.Count; i++)
        {
            if (downloadHistory[i]["id"] == id &&
                downloadHistory[i]["type"] == type)
            {
                downloadHistory[i] = metadata;
                return;
            }
        }

        downloadHistory.Add(metadata);
    }
}
