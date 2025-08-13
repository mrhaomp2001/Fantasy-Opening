using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    public void Loaded()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
