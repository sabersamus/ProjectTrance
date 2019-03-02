using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System;
using static PlayerInteractContainerEventArgs.EventType;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(PlayerUI))]
public class Player : MonoBehaviour, IInventoryHolder
{


    [SerializeField] public KeyCode runKey = KeyCode.LeftShift;


    private int baseHealth = 100;
    private int baseStamina = 100;
    private int baseMeleeStrength = 100;
    private int baseWater = 100;
    private int baseFood = 100;




    [SerializeField] public float health;
    [SerializeField] public float stamina;
    [SerializeField] public int meleeStrength;
    [SerializeField] public int water;
    [SerializeField] public int food;
    [SerializeField] public int staminaDrain;
    [SerializeField] public int staminaRegen;


    [HideInInspector]
    private Inventory inventory;


    public bool isSprinting;
    private int maxStamina;

    public new Camera camera;
    public Transform hand;
    Animator handAnim;
    public float interactDistance;

    private bool takingDamage = false;

    public PlayerUI ui;

    private AudioSource _audio;

    public bool canSprint;

    public bool isInMenu
    {
        get;
        set;
    }

    #region Events

    public static event EventHandler<PlayerInteractContainerEventArgs> InteractContainer;

    #endregion



    void Start()
    {
        if (inventory == null)
        {
            inventory = new PlayerInventory(this, 32);
        }

        handAnim = hand.GetComponent<Animator>();
        ui = GetComponent<PlayerUI>();



        //TODO:  Check if new player
        //Maybe move this to a playerJoin event?
        if (true/*new player*/)
        {
            health = baseHealth;
            //TODO: Change this v
            maxStamina = baseStamina;
            stamina = baseStamina;
            meleeStrength = baseMeleeStrength;
            water = baseWater;
            food = baseFood;
            health = baseHealth;
        }


    }

    void checkInput()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            if (hit.collider.CompareTag("Container"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Container container = hit.collider.GetComponent<Container>();
                    if (!isInMenu)
                    {
                        InteractContainer(this, new PlayerInteractContainerEventArgs(this, container, OPEN));
                        isInMenu = true;
                    }
                    else
                    {
                        InteractContainer(this, new PlayerInteractContainerEventArgs(this, container, CLOSE));
                        isInMenu = false;
                    }
                }
            }
        }
    }

    void LateUpdate()
    {
        checkInput();
    }


    void Update()
    {
        interact();

        if (stamina == 0)
        {
            canSprint = false;
        }
        else
        {
            canSprint = true;
        }

        if (Input.GetKey(runKey))
        {
            isSprinting = true;
        }else
        {
            isSprinting = false;
        }


        if (!isSprinting){
            if (stamina >= 100)
            {
                stamina = 100;
            }
            else
            {
               stamina += (staminaRegen * Time.deltaTime);
            }
        }

    }

    public Inventory getInventory()
    {
        return this.inventory;
    }


    //Move to a better class to handle player input
    private void interact()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            if (hit.collider.tag == ("Harvest"))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Harvest harvest = hit.collider.GetComponent<Harvest>();
                    harvest.harvestMaterials();
                    ui.triggerEvent(harvest);
                    inventory.addItem(new ItemStack(harvest.matType, (int)harvest.harvestAmount));
                    _audio = harvest.auidoSource;
                    _audio.Play();
                }
            }
        }
    }



    public void printInventory()
    {


    }

    public void damage(float value)
    {
        health -= value;
    }

    public float heal(float value)
    {
        if (health + value >= 100) return 100;
        return health + value;
    }


    private bool checkDamage()
    {
        if (takingDamage)
        {
            //TODO: Damage event
            return true;
        }

        return false;
    }

}
