using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpSleep : PopUp
{
    private static PopUpSleep instance;

    public static PopUpSleep Instance { get => instance; set => instance = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void OnSleep()
    {
        GameController.Instance.NextDay();

        Hide();
    }
}
