using GameUtil;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class FishRateCanCatch
{
    [SerializeField] private TileBase tileTarget;
    [SerializeField] private List<ItemBase> srFishs;
    [SerializeField] private List<ItemBase> rFishs;
    [SerializeField] private List<ItemBase> cFishs;

    public TileBase TileTarget { get => tileTarget; set => tileTarget = value; }
    public List<ItemBase> SrFishs { get => srFishs; set => srFishs = value; }
    public List<ItemBase> RFishs { get => rFishs; set => rFishs = value; }
    public List<ItemBase> CFishs { get => cFishs; set => cFishs = value; }
}

public class FishingController : Singleton<FishingController>, IUpdatable
{
    [SerializeField] private bool isFishing;

    [SerializeField] private Transform transformFishingRig;
    [Header("--")]
    [SerializeField] private Transform transformFishingBobber;
    [SerializeField] private LineRenderer lineFishing;
    [SerializeField] private Transform transformFishingIndicator;
    [SerializeField] private SpriteRenderer spriteFishingIndicator;
    [Header("--")]
    [SerializeField] private RectTransform containerFishingUI;
    [SerializeField] private Transform containerFishingProgress;
    [Header("--")]
    [SerializeField] private List<FishRateCanCatch> fishRateCanCatchList;

    [SerializeField] private FishRateCanCatch fishRateCanCatchDefault;
    private ItemFishingRod itemFishingRod;
    private Timer timerFishing;
    public bool IsFishing { get => isFishing; set => isFishing = value; }

    private void Start()
    {
        containerFishingProgress.gameObject.SetActive(false);
        transformFishingRig.gameObject.SetActive(false);

    }
    public void UpdateViews()
    {

        // Fishing
        transformFishingIndicator.gameObject.SetActive(false);

        containerFishingUI.gameObject.SetActive(false);

        var itemHolding = InventoryController.Instance.GetPlayerData.SelectedHotbar.item;
        if (itemHolding is ItemFishingRod fishingRod)
        {
            transformFishingIndicator.gameObject.SetActive(true);

#if UNITY_ANDROID || UNITY_IOS

            containerFishingUI.gameObject.SetActive(true);
#endif

        }
    }


    public void OnClickFishing(ItemFishingRod itemFishing)
    {
        if (!FishingController.Instance.IsFishing)
        {
            itemFishingRod = itemFishing;
            FishingController.Instance.StartFishing();
        }
        else
        {
            FishingController.Instance.StopFishing();
        }
    }

    public void StartFishing()
    {
        if (!IsFshingValid())
        {
            return;
        }

        spriteFishingIndicator.gameObject.SetActive(false);
        containerFishingProgress.gameObject.SetActive(true);

        isFishing = true;

        transformFishingRig.gameObject.SetActive(true);

        UpdateLine();

        float fishingTime = UnityEngine.Random.Range(itemFishingRod.Stats.MinFishingTime, itemFishingRod.Stats.MaxFishingTime);

        timerFishing = Timer.DelayAction(fishingTime,
        onComplete: () =>
        {
            OnCatchFish();
        },
        onUpdate: (float ratio) =>
        {

        });
    }

    public void OnCatchFish()
    {
        StopFishing();

        InventoryController.Instance.ConsumeHunger(itemFishingRod.Stats.HungerCost);

        int totalChance = itemFishingRod.Stats.SuperRareFishRate + itemFishingRod.Stats.RareFishRate + itemFishingRod.Stats.CommonFishRate;
        int result = UnityEngine.Random.Range(0, totalChance);

        Debug.Log("Fishing Result: " + result);

        TileBase currentTile = RuleTileDetector.Instance.GetCurrentValidTile();

        FishRateCanCatch fishRateCanCatch = fishRateCanCatchList
            .Where(predicate =>
            {
                return predicate.TileTarget == currentTile;
            })
            .FirstOrDefault();

        if (fishRateCanCatch == null)
        {
            fishRateCanCatch = fishRateCanCatchDefault;
        }

        if (result < itemFishingRod.Stats.SuperRareFishRate)
        {
            GetSuperRareFish(fishRateCanCatch);
            return;
        }
        if (result < (itemFishingRod.Stats.SuperRareFishRate + itemFishingRod.Stats.RareFishRate))
        {
            GetRareFish(fishRateCanCatch);
            return;
        }

        if (result < (itemFishingRod.Stats.SuperRareFishRate + itemFishingRod.Stats.RareFishRate + itemFishingRod.Stats.CommonFishRate))
        {
            GetCommonFish(fishRateCanCatch);
            return;
        }
    }

    private void GetSuperRareFish(FishRateCanCatch fishRateCanCatch)
    {
        if (fishRateCanCatch == null || fishRateCanCatch.SrFishs == null || fishRateCanCatch.SrFishs.Count == 0)
        {
            return;
        }
        ItemBase fish = fishRateCanCatch.SrFishs[UnityEngine.Random.Range(0, fishRateCanCatch.SrFishs.Count)];
        SpawnItemFish(fish);
        Debug.Log("Get Super Rare Fish");
    }

    private void GetRareFish(FishRateCanCatch fishRateCanCatch)
    {

        if (fishRateCanCatch == null || fishRateCanCatch.RFishs == null || fishRateCanCatch.RFishs.Count == 0)
        {
            return;
        }
        ItemBase fish = fishRateCanCatch.RFishs[UnityEngine.Random.Range(0, fishRateCanCatch.RFishs.Count)];
        SpawnItemFish(fish);
        Debug.Log("Get Rare Fish");
    }


    private void GetCommonFish(FishRateCanCatch fishRateCanCatch)
    {
        if (fishRateCanCatch == null || fishRateCanCatch.CFishs == null || fishRateCanCatch.CFishs.Count == 0)
        {
            return;
        }
        ItemBase fish = fishRateCanCatch.CFishs[UnityEngine.Random.Range(0, fishRateCanCatch.CFishs.Count)];

        SpawnItemFish(fish);
        Debug.Log("Get Common Fish");
    }


    public void SpawnItemFish(ItemBase itemFish)
    {
        WorldItemController.Instance.SpawnItem(itemFish.Id, PlayerController.Instance.RbPlayer.transform.position);
    }

    public void StopFishing()
    {
        if (isFishing)
        {
            isFishing = false;

            transformFishingRig.gameObject.SetActive(false);

            containerFishingProgress.gameObject.SetActive(false);


            spriteFishingIndicator.gameObject.SetActive(true);

            Timer.Cancel(timerFishing);
        }

    }

    public void UpdateLine()
    {
        lineFishing.SetPosition(0, PlayerController.Instance.SpriteItemHolding.transform.position);
        lineFishing.SetPosition(1, transformFishingBobber.position);
    }

    public bool IsFshingValid()
    {
        return BuildingController.Instance.IsCellEmpty();
    }

    public void OnUpdate()
    {
        if (InventoryController.Instance != null)
        {
            if (InventoryController.Instance.GetPlayerData != null && InventoryController.Instance.GetPlayerData.SelectedHotbar != null)
            {
                if (InventoryController.Instance.GetPlayerData.SelectedHotbar.item != null)
                {
                    if (InventoryController.Instance.GetPlayerData.SelectedHotbar.item is ItemFishingRod fishingRod)
                    {
                        Vector3Int cellPosition = BuildingController.Instance.GridBuilding.WorldToCell(PlayerController.Instance.FirepointHitbox.transform.position);

                        transformFishingIndicator.transform.position = BuildingController.Instance.GridBuilding.GetCellCenterWorld(cellPosition);
                    }

                    if (BuildingController.Instance.IsCellEmpty())
                    {
                        spriteFishingIndicator.color = Color.green;
                    }
                    else
                    {
                        spriteFishingIndicator.color = Color.red;
                    }
                }
            }
        }
    }

    private void OnEnable()
    {
        UpdateController.Instance.Updatables.Add(this);
    }

    private void OnDisable()
    {
        UpdateController.Instance.Updatables.Remove(this);
    }

    private void OnDestroy()
    {
        Timer.Cancel(timerFishing);
    }
}
