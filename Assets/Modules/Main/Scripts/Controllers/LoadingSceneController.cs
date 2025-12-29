using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneIndex
{
    Loading = 0,
    MainMenu = 1,
    Gameplay = 2
}

public class LoadingSceneController : MonoBehaviour
{
    public void Loaded()
    {
        SceneManager.LoadScene((int)SceneIndex.MainMenu);
    }

}
