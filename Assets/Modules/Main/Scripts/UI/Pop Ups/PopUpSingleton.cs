using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpSingleton<T> : PopUp where T : PopUp
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    Debug.LogError($"{typeof(T).Name} not found in scene!");
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
}
