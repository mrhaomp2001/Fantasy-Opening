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

    public override void OnSave()
    {
        if (IsCompleted && !IsSaved)
        {
            base.OnSave();
            imageBlocker.gameObject.SetActive(true);
            imageBlocker.color = new Color(0f, 0f, 0f, 1f);

            PopUpDialogue.Instance.ShowDialogue(event_3_result);
            enchantedPea.gameObject.SetActive(true);
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
        }
    }
    public void FadeOutImage()
    {
        LeanTween.cancel(imageBlocker.gameObject);
        LeanTween.value(imageBlocker.gameObject, 1f, 0f, 2f)
            .setOnUpdate((float value) =>
            {
                imageBlocker.color = new Color(0f, 0f, 0f, value);
            })
            .setOnComplete(() =>
            {
                imageBlocker.gameObject.SetActive(false);
            });
    }
}
