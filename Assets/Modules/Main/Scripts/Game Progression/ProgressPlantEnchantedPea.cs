using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ProgressPlantEnchantedPea : ProgressionBase
{
    [SerializeField] private Image imageBlocker;
    [SerializeField] private List<Dialogue> event_3_result;
    [SerializeField] private Transform enchantedPea;
    [SerializeField] private Transform[] christmasWorld2025;

    public override void OnSave()
    {
        if (IsCompleted && !IsSaved)
        {
            base.OnSave();
            PopUpRaycastBlocker.Instance.Show();


            PopUpDialogue.Instance.ShowDialogue(event_3_result);
            enchantedPea.gameObject.SetActive(true);

            ShowChristmas();

            var enemies = FindObjectsByType<Enemy>(findObjectsInactive: FindObjectsInactive.Include, sortMode: FindObjectsSortMode.None);

            foreach (var item in enemies)
            {
                item.CanMove = false;
            }
        }

        RunNextEvent();
    }

    public void RunNextEvent()
    {
        if (IsSaved)
        {
            var progression = ProgressionController.Instance.Progressions
            .Where(predicate =>
            {
                return predicate.ProgressionName.Equals("event_4_0");
            })
            .FirstOrDefault();

            if (progression != null)
            {
                progression.OnReady();
            }
        }
    }

    public override void OnLoad()
    {
        if (IsSaved)
        {
            enchantedPea.gameObject.SetActive(true);
            ShowChristmas();
        }
    }

    private void ShowChristmas()
    {
        foreach (var item in christmasWorld2025)
        {
            item.gameObject.SetActive(true);
        }
    }

    public void FadeOutImage()
    {
        PopUpRaycastBlocker.Instance.Hide();

        var enemies = FindObjectsByType<Enemy>(findObjectsInactive: FindObjectsInactive.Include, sortMode: FindObjectsSortMode.None);

        foreach (var item in enemies)
        {
            item.CanMove = true;
        }

    }
}
