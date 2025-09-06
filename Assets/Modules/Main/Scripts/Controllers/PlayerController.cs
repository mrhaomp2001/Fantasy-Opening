using GameUtil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IUpdatable, IFixedUpdatable
{
    private static PlayerController instance;

    [SerializeField] private float speed;

    [SerializeField] private Rigidbody2D rbPlayer;
    [SerializeField] private SpriteRenderer spritePlayer;
    [SerializeField] private SpriteRenderer spriteItemHolding;
    [SerializeField] private Animator animator;

    [Header("Fire Point: ")]
    [SerializeField] private int cheatItemId;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform firepoint;
    [SerializeField] private Transform firepointHitbox;
    [Header("Attack: ")]
    [SerializeField] private float attackCooldown;

    private bool canAttack;
    private Timer timerAttack;

    [Header("Hurt: ")]
    [SerializeField] private bool canHurt, isHurt;
    [SerializeField] private float hurtCooldown, hurtTime;
    [SerializeField] private RectTransform deadScreen;


    [Header("Interact: ")]
    [SerializeField] private bool isHolding;
    [SerializeField] private LinkedList<IWorldInteractable> interactable = new();



    private Vector2 movementSpeed;

    public static PlayerController Instance { get => instance; set => instance = value; }
    public Transform FirepointHitbox { get => firepointHitbox; set => firepointHitbox = value; }
    public Rigidbody2D RbPlayer { get => rbPlayer; set => rbPlayer = value; }
    public float Speed { get => speed; set => speed = value; }
    public SpriteRenderer SpriteItemHolding { get => spriteItemHolding; set => spriteItemHolding = value; }

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
    private void Start()
    {
        canAttack = true;

        canHurt = true;
        isHurt = false;
    }

    public void OnUpdate()
    {
        TestFunction();

        spritePlayer.sortingOrder = (int)-(transform.position.y * 100f) + 1;
        Movement();

        FirePointCalculation();

        if (Input.GetKeyDown(GameInputController.Instance.Hotkey1.keyCode))
        {
            PopUpInventory.Instance.HotbarItem[0].OnClick();
        }

        if (Input.GetKeyDown(GameInputController.Instance.Hotkey2.keyCode))
        {

            PopUpInventory.Instance.HotbarItem[1].OnClick();
        }

        if (Input.GetKeyDown(GameInputController.Instance.Hotkey3.keyCode))
        {
            PopUpInventory.Instance.HotbarItem[2].OnClick();

        }

        if (Input.GetKeyDown(GameInputController.Instance.Hotkey4.keyCode))
        {
            PopUpInventory.Instance.HotbarItem[3].OnClick();

        }

        if (Input.GetKeyDown(GameInputController.Instance.Hotkey5.keyCode))
        {
            PopUpInventory.Instance.HotbarItem[4].OnClick();

        }

        if (Input.GetKeyDown(GameInputController.Instance.Hotkey6.keyCode))
        {
            PopUpInventory.Instance.HotbarItem[5].OnClick();

        }

        if (Input.GetKeyDown(GameInputController.Instance.Hotkey7.keyCode))
        {
            PopUpInventory.Instance.HotbarItem[6].OnClick();

        }

        if (Input.GetKeyDown(GameInputController.Instance.Hotkey8.keyCode))
        {
            PopUpInventory.Instance.HotbarItem[7].OnClick();

        }

        if (Input.GetKeyDown(GameInputController.Instance.Hotkey9.keyCode))
        {
            PopUpInventory.Instance.HotbarItem[8].OnClick();

        }

        if (Input.GetKeyDown(GameInputController.Instance.Inventory.keyCode))
        {
            PopUpInventory.Instance.TurnPopUp();
        }

        OnHolding();

    }

    private void OnHolding()
    {
        if (isHolding)
        {
            if (!PopUpInventory.Instance.IsOpening)
            {
                InteractWithItem();
            }
        }
    }

    private void TestFunction()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            GameInputController.Instance.Save();
            InventoryController.Instance.Save();
        }
        if (Input.GetKeyDown(KeyCode.F7))
        {
            PlayerPrefs.DeleteAll();
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            Debug.Log("Loading Player Data");
            InventoryController.Instance.Load();
        }

        if (Input.GetKeyDown(KeyCode.F8))
        {
            foreach (var item in ItemDatabase.Instance.Items)
            {
                InventoryController.Instance.Add(item.Id, 20);
            }
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            GameController.Instance.NextDay();
        }

        if (Input.GetKeyDown(KeyCode.F10))
        {
            WorldItemController.Instance.SpawnItem(cheatItemId, firepointHitbox.position);
        }



        if (Input.GetKeyDown(KeyCode.F11))
        {
            ObjectPooler.Instance.DeactivateAllObjects();
            SceneManager.LoadScene((int)SceneIndex.MainMenu);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (interactable.First != null)
            {
                if (interactable.First.Value is BuildingBase building)
                {
                    BuildingController.Instance.DestroyBuilding(building.Id);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (interactable.First != null)
            {
                if (interactable.First.Value is BuildingFarmland farmland)
                {
                    farmland.OnNextDay();
                }
            }
        }
    }

    public void OnFixedUpdate()
    {
        rbPlayer.velocity = movementSpeed * speed;
    }

    private void OnEnable()
    {
        UpdateController.Instance.Updatables.Add(this);
        UpdateController.Instance.FixedUpdateables.Add(this);
    }

    private void OnDisable()
    {
        UpdateController.Instance.Updatables.Remove(this);
        UpdateController.Instance.FixedUpdateables.Remove(this);
    }

    private void Movement()
    {
        if (Input.GetKey(GameInputController.Instance.Up.keyCode))
        {
            movementSpeed.y = 1;
        }
        else if (Input.GetKey(GameInputController.Instance.Down.keyCode))
        {
            movementSpeed.y = -1;
        }
        else
        {

            movementSpeed.y = 0;
        }

        if (Input.GetKey(GameInputController.Instance.Left.keyCode))
        {
            movementSpeed.x = -1;
            spritePlayer.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (Input.GetKey(GameInputController.Instance.Right.keyCode))
        {
            movementSpeed.x = 1;
            spritePlayer.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            movementSpeed.x = 0;
        }

        if (movementSpeed != Vector2.zero)
        {
            animator.SetInteger("state", 1);
        }
        else
        {
            animator.SetInteger("state", 0);
        }
    }

    public void FirePointCalculation()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        Vector3 targetPosition = mousePos - firepoint.transform.position;

        float targetRotation = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;

        firepoint.rotation = Quaternion.Euler(0, 0, targetRotation);

        firepointHitbox.transform.localPosition = Vector3.right * Mathf.Clamp(Vector3.Distance(mousePos, new Vector3(firepoint.transform.position.x, firepoint.transform.position.y, 0f)), 0.2f, 3f);
    }

    public void InteractWithItem()
    {
        if (canAttack)
        {
            //AudioController.Instance.Play("player_attack");tt
            CooldownAction();

            if (InventoryController.Instance.GetPlayerData.SelectedHotbar.item != null)
            {
                if (InventoryController.Instance.GetPlayerData.SelectedHotbar.item is ItemWeapon weapon)
                {
                    ObjectPooler.Instance.SpawnFromPool(weapon.Projectile, transform.position, firepointHitbox.rotation);
                }
            }

            if (InventoryController.Instance.GetPlayerData.SelectedHotbar.item == null)
            {
                ObjectPooler.Instance.SpawnFromPool("player_bullet", transform.position, firepointHitbox.rotation);
            }
        }
    }

    private void CooldownAction()
    {
        float cooldownTime = 0f;
        if (InventoryController.Instance.GetPlayerData.SelectedHotbar.item == null || InventoryController.Instance.GetPlayerData.SelectedHotbar.item.UseTime <= 0)
        {
            cooldownTime = attackCooldown;
        }
        else
        {
            float percent = Mathf.Min(InventoryController.Instance.GetPlayerData.AttackSpeed, 75f);
            float offset = InventoryController.Instance.GetPlayerData.SelectedHotbar.item.UseTime * (percent / 100f);

            cooldownTime = InventoryController.Instance.GetPlayerData.SelectedHotbar.item.UseTime - offset;
        }

        canAttack = false;
        timerAttack = Timer
            .DelayAction(cooldownTime, () =>
            {
                canAttack = true;
            });

    }

    public void SleepNextDay()
    {
        GameController.Instance.NextDay();
    }
    public void Hurt(int dmg)
    {
        if (canHurt && dmg > 0)
        {
            canHurt = false;
            Timer.DelayAction(hurtCooldown, () =>
            {
                canHurt = true;
            });

            isHurt = true;
            Timer.DelayAction(hurtTime, () =>
            {
                isHurt = false;
            });

            animator.Play("hurt");

            Debug.Log($"Defend: {InventoryController.Instance.GetPlayerData.Defend}");

            InventoryController.Instance.GetPlayerData.Hp -= Mathf.Max(1, dmg - InventoryController.Instance.GetPlayerData.Defend);

            StatController.Instance.UpdateHp();

            if (InventoryController.Instance.GetPlayerData.Hp <= 0)
            {
                deadScreen.gameObject.SetActive(true);

                var enemy = FindObjectsOfType<Enemy>();

                for (int i = 0; i < enemy.Length; i++)
                {
                    enemy[i].Despawn();
                }
            }
        }
    }

    public void OnRevive()
    {
        InventoryController.Instance.GetPlayerData.Hp = 100;
        deadScreen.gameObject.SetActive(false);

        StatController.Instance.UpdateViews();
    }

    public void OnTouchEnemy(Collider2D other)
    {
        var enemy = other.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            Hurt(enemy.TouchDamage);
        }

    }

    // Interact
    public void OnEnterWorldInteract(Collider2D other)
    {
        interactable.AddFirst(other.GetComponentInParent<IWorldInteractable>());
    }

    public void OnExitWorldInteract(Collider2D other)
    {
        if (interactable.Count > 0)
        {
            interactable.RemoveLast();
        }
    }

    // World Item

    public void OnEnterWorldItem(Collider2D other)
    {
        var item = other.GetComponentInParent<WorldItem>();
        item.OnPlayerTouch();
    }

    public void OnPointerDown(BaseEventData valueEventData)
    {

        if (valueEventData is PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                isHolding = true;
                if (InventoryController.Instance.GetPlayerData.SelectedHotbar.item != null && InventoryController.Instance.GetPlayerData.SelectedHotbar.item is ItemWeapon weaponHammer)
                {
                    if (weaponHammer.WeaponType == WeaponType.Hammer)
                    {
                        return;
                    }
                }
                if (interactable.First != null)
                {
                    if (interactable.First.Value is BuildingFarmland farmland)
                    {
                        if (InventoryController.Instance.GetPlayerData.SelectedHotbar.item is ItemSeed seed)
                        {
                            if (farmland.OnSowSeed(seed.CropId))
                            {
                                InventoryController.Instance.Consume(seed.Id, 1, new Callback
                                {
                                    onSuccess = () =>
                                    {

                                    },
                                    onFail = (message) =>
                                    {

                                    },
                                    onNext = () =>
                                    {
                                    }
                                });
                            }
                        }


                    }
                    interactable.First.Value.OnWorldInteract();
                }

                if (InventoryController.Instance.GetPlayerData.SelectedHotbar.item is ItemBuilding building)
                {
                    if (BuildingController.Instance.IsBuildValid())
                    {
                        InventoryController.Instance.Consume(building.Id, 1, new Callback
                        {
                            onSuccess = () =>
                            {
                                BuildingController.Instance.Build(building.BuildingName);

                            },
                            onFail = (message) =>
                            {

                            },
                            onNext = () =>
                            {
                            }
                        });
                    }
                }

                if (InventoryController.Instance.GetPlayerData.SelectedHotbar.item is ItemBossSummon bossSummon)
                {
                    bossSummon.OnSummonBoss();
                }

                if (InventoryController.Instance.GetPlayerData.SelectedHotbar.item is ItemRecipe recipe)
                {
                    recipe.UnlockRecipe();
                }

            }
            if (eventData.button == PointerEventData.InputButton.Right)
            {

            }
            if (eventData.button == PointerEventData.InputButton.Middle)
            {

            }
        }
    }

    public void OnPointerUp(BaseEventData valueEventData)
    {
        if (valueEventData is PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                isHolding = false;
            }
            if (eventData.button == PointerEventData.InputButton.Right)
            {

            }
            if (eventData.button == PointerEventData.InputButton.Middle)
            {

            }
        }
    }

}
