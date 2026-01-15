using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpAscension : PopUpSingleton<PopUpAscension>
{
    private void Start()
    {
        Debug.Log($"Level: {WitchSystemController.Instance.Data.Level} - WitchMedal: {WitchSystemController.Instance.Data.WitchMedal}");
    }
    public void Ascension()
    {
        InventoryController.Instance.Ascension();

    }
}
