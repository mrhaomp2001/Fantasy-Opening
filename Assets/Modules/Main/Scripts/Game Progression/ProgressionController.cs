using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionController : MonoBehaviour
{
    private static ProgressionController instance;

    [SerializeField] private List<ProgressionBase> progressions;

    public List<ProgressionBase> Progressions { get => progressions; set => progressions = value; }
    public static ProgressionController Instance { get => instance; set => instance = value; }

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
}
