using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSceneController : MonoBehaviour
{
    private static bool isLoadData;

    public static bool IsLoadData { get => isLoadData; set => isLoadData = value; }

    public void OnStart()
    {
        isLoadData = false;

        SceneManager.LoadScene((int)SceneIndex.Gameplay);

        AudioController.Instance.PlayButton();
    }

    public void OnLoadGame()
    {
        isLoadData = true;

        SceneManager.LoadScene((int)SceneIndex.Gameplay);

        AudioController.Instance.PlayButton();
    }

    public void OnOpenSettings()
    {
        PopUpSetting.Instance.Show();

        AudioController.Instance.PlayButton();
    }
}