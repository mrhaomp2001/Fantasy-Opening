using GameUtil;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PopUpSeaColorChanger : PopUpSingleton<PopUpSeaColorChanger>
{
    [SerializeField] private Color defaultSeaColor;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private FlexibleColorPicker seaColorPicker;
    [SerializeField] private Tilemap[] tilemapWaters;

    public void OnLoad()
    {

        Debug.Log($"SeaColorHex: {PopUpSetting.Instance.SeaColorHex}");
        Debug.Log($"--");

        if (ColorUtility.TryParseHtmlString("#" + PopUpSetting.Instance.SeaColorHex, out Color colorOutput))
        {
            mainCamera.backgroundColor = colorOutput;

            seaColorPicker.SetColorNoAlpha(colorOutput);

            foreach (var tilemap in tilemapWaters)
            {
                tilemap.color = colorOutput;
            }
        Debug.Log($"{colorOutput}");
        }


        Debug.Log($"{nameof(PopUpSeaColorChanger)} 2");

    }

    public override void Show()
    {
        base.Show();

        if (ColorUtility.TryParseHtmlString("#" + PopUpSetting.Instance.SeaColorHex, out Color colorOutput))
        {
            mainCamera.backgroundColor = colorOutput;

            seaColorPicker.SetColorNoAlpha(colorOutput);

            foreach (var tilemap in tilemapWaters)
            {
                tilemap.color = colorOutput;
            }
        }
    }

    public override void Hide()
    {
        base.Hide();
        PopUpSetting.Instance.SeaColorHex = ColorUtility.ToHtmlStringRGB(seaColorPicker.color);
        PopUpSetting.Instance.Save();
    }

    public void OnColorChanged(Color newColor)
    {
        mainCamera.backgroundColor = newColor;

        foreach (var tilemap in tilemapWaters)
        {
            tilemap.color = newColor;
        }
    }

    public void DefaultColorButton()
    {
        seaColorPicker.SetColorNoAlpha(defaultSeaColor);
        mainCamera.backgroundColor = defaultSeaColor;

        foreach (var tilemap in tilemapWaters)
        {
            tilemap.color = defaultSeaColor;
        }
    }
}
