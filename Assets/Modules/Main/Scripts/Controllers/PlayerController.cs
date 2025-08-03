using GameUtil;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.VersionControl;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;

    [SerializeField] private float speed;

    [SerializeField] private Rigidbody2D rbPlayer;

    [Header("Fire Point: ")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform firepoint;
    [SerializeField] private Transform firepointHitbox;
    [Header("Attack: ")]
    [SerializeField] private bool isFiring;
    [SerializeField] private float attackCooldown;
    private bool canAttack;
    private Timer timerAttack;
    [Header("Interact: ")]
    [SerializeField] private IWorldInteractable interactable;
    [Header("Test: ")]
    [SerializeField] private Crop crop1;
    [SerializeField] private Crop crop2, crop3, crop4;



    private Vector2 movementSpeed;

    public static PlayerController Instance { get => instance; set => instance = value; }

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        canAttack = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            GameInputController.Instance.Save();
        }
        if (Input.GetKeyDown(KeyCode.F7))
        {
            PlayerPrefs.DeleteAll();
        }

        Movement();

        FirePointCalculation();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (interactable != null)
            {
                if (interactable is Farmland farmland)
                {
                    farmland.OnSowSeed(crop1);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (interactable != null)
            {
                if (interactable is Farmland farmland)
                {

                    farmland.OnSowSeed(crop2);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (interactable != null)
            {
                if (interactable is Farmland farmland)
                {
                    farmland.OnSowSeed(crop3);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (interactable != null)
            {
                if (interactable is Farmland farmland)
                {
                    farmland.OnSowSeed(crop4);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            InventoryController.Instance.Add(3, 1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            InventoryController.Instance.Consume(3, 1, new Callback
            {
                onSuccess = () =>
                {
                    Debug.Log("success");

                },
                onFail = (message) =>
                {
                    Debug.Log(message);
                },
                onNext = () =>
                {
                    Debug.Log("next");

                }
            });
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            InventoryController.Instance.Add(403, 1);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            isFiring = !isFiring;
        }
        if (Input.GetMouseButtonDown(0))
        {

            if (interactable != null)
            {
                if (interactable is Farmland)
                {
                    interactable.OnWorldInteract();
                }
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (isFiring)
            {
                Attack();
            }
        }

        if (Input.GetKeyDown(GameInputController.Instance.Inventory.keyCode))
        {
            PopUpInventory.Instance.TurnPopUp();
        }
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
        }
        else if (Input.GetKey(GameInputController.Instance.Right.keyCode))
        {
            movementSpeed.x = 1;
        }
        else
        {
            movementSpeed.x = 0;
        }
    }

    public void FirePointCalculation()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        Vector3 targetPosition = mousePos - firepoint.transform.position;

        float targetRotation = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;

        firepoint.rotation = Quaternion.Euler(0, 0, targetRotation);

        firepointHitbox.transform.localPosition = Vector3.right * Vector3.Distance(mousePos, new Vector3(firepoint.transform.position.x, firepoint.transform.position.y, 0f));
    }

    private void FixedUpdate()
    {
        rbPlayer.velocity = movementSpeed * speed;
    }

    public void Attack()
    {
        if (canAttack)
        {
            //AudioController.Instance.Play("player_attack");

            canAttack = false;
            timerAttack = Timer.DelayAction(attackCooldown, () =>
            {
                canAttack = true;
            });

            ObjectPooler.Instance.SpawnFromPool("player_bullet", transform.position, firepointHitbox.rotation);
        }
    }


    // Interact
    public void OnEnterWorldInteract(Collider2D other)
    {
        interactable = other.GetComponentInParent<IWorldInteractable>();
    }

    public void OnExitWorldInteract(Collider2D other)
    {
        interactable = null;
    }

    // World Item

    public void OnEnterWorldItem(Collider2D other)
    {
        var item = other.GetComponentInParent<WorldItem>();
        item.OnPlayerTouch();
    }
}
