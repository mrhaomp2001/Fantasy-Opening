using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSceneController : MonoBehaviour
{
    public void OnStart()
    {
        SceneManager.LoadScene((int)SceneIndex.Gameplay);
    }
}