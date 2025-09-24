using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopUpUpgradeSelector : PopUp
{
    private static PopUpUpgradeSelector instance;
    [SerializeField] private List<BuffCardviewItem> buffCardviewItems;

    public static PopUpUpgradeSelector Instance { get => instance; set => instance = value; }

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

    public void ShowUpgrade()
    {
        base.Show();

        var randomBuffs = ItemDatabase.Instance
            .Buffs
                .OrderBy(x => Random.value)
                .Take(3)
                .ToList();

        for (int i = 0; i < 3; i++)
        {
            buffCardviewItems[i].UpdateViews(randomBuffs[i]);
        }
    }

    public void OnChoiceBuff(BuffBase buffValue)
    {
        //Debug.Log($"{buffValue.BuffName}");
        base.Hide();
        GameController.Instance.OnSelectedBuff(buffValue);
    }
}
