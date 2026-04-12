using GameUtil;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, JsonObject(MemberSerialization.OptIn)]
public class BuildingWithCraftingAndTime : BuildingBase, IWorldInteractable, IPoolObject
{
    [System.Serializable, JsonObject(MemberSerialization.OptIn)]
    public class RecipeWithTime : Recipe
    {
        [SerializeField] private int time;

        public int Time { get => time; set => time = value; }
    }
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [Header("Progress: ")]
    [SerializeField] private GameObject progressContainer;
    [SerializeField] private GameObject progressBarBg;
    [SerializeField] private SpriteRenderer spriteRendererProgress;
    [SerializeField] private SpriteRenderer spriteRendererResult;

    [SerializeField] private List<RecipeWithTime> recipes;

    [SerializeField] private int currentTime;
    [JsonProperty][SerializeField] private InventoryController.InventoryItem targetResult;

    private Timer timerCrafting;

    public InventoryController.InventoryItem TargetResult { get => targetResult; set => targetResult = value; }

    public void OnObjectSpawnAfter()
    {
        spriteRenderer.sortingOrder = (int)-(transform.position.y * 100f);

        UpdateViews();

    }

    public override void OnWorldInteract()
    {
        if (currentTime <= 0)
        {
            if (TargetResult.item == null )
            {
                PopUpInventory.Instance.TurnPopUp();

                PopUpInventory.Instance.TurnCrafting(recipes, station: this, isHideInventoryOptions: true);
            }
            else
            {
                GetResult();
            }
        }

    }

    public void GetResult()
    {
        WorldItemController.Instance.SpawnItem(TargetResult.item.Id, transform.position, TargetResult.count);

        TargetResult.item = null;

        UpdateViews();

    }

    public void StartCrafting(RecipeWithTime recipe)
    {
        TargetResult = new InventoryController.InventoryItem
        {
            count = recipe.ResultCount,
            item = recipe.ItemResult,
        };

        spriteRendererProgress.gameObject.transform.localScale = new Vector3(0f, 1f, 1f);


        float maxTime = recipe.Time;



        timerCrafting = Timer.DelayAction(recipe.Time,
            onComplete: () =>
            {
                currentTime = 0;

                Timer.Cancel(timerCrafting);
                timerCrafting = null;

                UpdateViews();
            },
            onUpdate: (float value) =>
            {
                currentTime = (int)value + 1;
                float normalized = Mathf.Clamp01(value / maxTime);

                spriteRendererProgress.transform.localScale = new Vector3(normalized, 1f, 1f);
            });

        UpdateViews();

    }

    public void UpdateViews()
    {
        if (timerCrafting == null)
        {
            animator.Play("idle");

            progressBarBg.gameObject.SetActive(false);


        }
        else
        {
            animator.Play("running");
            progressBarBg.gameObject.SetActive(true);
        }

        if (TargetResult.item != null)
        {
            progressContainer.SetActive(true);

            spriteRendererResult.sprite = targetResult.item.Sprite;

        }
        else
        {
            progressContainer.gameObject.SetActive(false);

        }
    }

    private void OnDisable()
    {
        Timer.Cancel(timerCrafting);
    }
}
