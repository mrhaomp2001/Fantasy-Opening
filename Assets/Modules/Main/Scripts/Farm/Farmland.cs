using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Farmland : MonoBehaviour, IWorldInteractable
{
    [SerializeField] private int currentDay;
    [SerializeField] private Crop cropCurrent;
    [SerializeField] private Sprite emptySprite;
    [Header("UI: ")]

    [SerializeField] private SpriteRenderer imageCrop;

    public Crop CropCurrent { get => cropCurrent; set => cropCurrent = value; }
    public int CurrentDay { get => currentDay; set => currentDay = value; }

    public virtual void OnNextDay()
    {
        currentDay++;

        UpdateViews();
    }

    public void UpdateViews()
    {
        if (cropCurrent == null)
        {
            imageCrop.sprite = emptySprite;
            currentDay = 0;
        }
        else
        {
            imageCrop.sprite = cropCurrent.Stages[currentDay < cropCurrent.Stages.Count ? currentDay : cropCurrent.Stages.Count - 1].sprite;
        }
    }

    public bool OnSowSeed(int cropId)
    {
        if (cropCurrent == null)
        {
            ObjectPooler.Instance.SpawnFromPool("harvest_effect", transform.position, transform.rotation);

            Crop value = ItemDatabase.Instance.Crop
                .Where(predicate => { return predicate.Id == cropId; })
                .FirstOrDefault();

            cropCurrent = value;
            UpdateViews();

            return true;
        }

        return false;

    }

    public void OnHarvest()
    {
        if (cropCurrent != null)
        {
            if (currentDay >= cropCurrent.Stages.Count - 1)
            {
                ObjectPooler.Instance.SpawnFromPool("harvest_effect", transform.position, Quaternion.identity);

                //InventoryController.Instance.Add(cropCurrent.ProductId, 1);

                WorldItemController.Instance.SpawnItem(cropCurrent.ProductId, transform.position);

                cropCurrent = null;

                currentDay = 0;

                UpdateViews();
            }
            else
            {
                Debug.Log("Chưa chín!");
            }
        }
    }

    public void OnWorldInteract()
    {
        if (cropCurrent != null)
        {
            OnHarvest();
        }
        else
        {

        }
    }
}
