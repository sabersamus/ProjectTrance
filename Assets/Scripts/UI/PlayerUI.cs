using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{


    public Player player;

    //UI elements
    public Canvas inventoryPanel;
    public Canvas statsUi;
    public Canvas hotbar;
    public Canvas equipment;
    public Canvas timeOfDay;
    public Canvas backpack;
    public Canvas container;
    public Text text;
    public Slider healthBar;
    public Slider staminaBar;
    public Slider foodBar;
    public Slider waterBar;

    public Slot[] inventorySlots;

    public GameObject inventorySlotsHolder;

    private void Awake()
    {
        Inventory.InventoryChanged += OnChangeUI;
        Player.InteractContainer += OnPlayerOpenContainer;
    }

    private void OnDisable()
    {
        Inventory.InventoryChanged -= OnChangeUI;
        Player.InteractContainer -= OnPlayerOpenContainer;
    }

    #region Events


    private void OnChangeUI(object sender, InventoryChangeEventArgs eventArgs)
    {
        if (eventArgs.inventory != player.getInventory()) return;

        Debug.Log("Its the players inventory");

    }

    private bool containerOpen = false;
    private void OnPlayerOpenContainer(object sender, PlayerInteractContainerEventArgs eventArgs)
    {
        Inventory containerInventory = eventArgs.container.getInventory();

        bool open = eventArgs.eventType == PlayerInteractContainerEventArgs.EventType.OPEN;
        containerOpen = open;
        Debug.Log("Was the inventory previously open? : " + inventoryOpen);
        if (!inventoryOpen)
        {
            inventoryOpen = true;
        }
        else
        {
            inventoryOpen = open;
        }

        Debug.Log("Open type event: " + open);
        Debug.Log("Is the inventory open now");

        setUI(inventoryOpen, containerOpen);

    }

    #endregion


    private void setUI(bool inventory, bool _container)
    {
        inventoryPanel.enabled = inventory;
        statsUi.enabled = !inventory;
        equipment.enabled = inventory;
        backpack.enabled = inventory;
        container.enabled = _container;
    }


    //Private fields
    private bool inventoryOpen;

    public static int getHotbarSize()
    {
        return 10;
    }



    // Start is called before the first frame update
    void Start()
    {
        //healthBar = GetComponent<Slider>();
        inventorySlots = new Slot[(inventorySlotsHolder.transform.childCount)];

        for(int i=0; i < player.getInventory().maxSize; i++)
        {
            inventorySlots[i] = inventorySlotsHolder.transform.GetChild(i).GetComponent<Slot>();
        }

        inventoryOpen = false;
        inventoryPanel.enabled = false;
        equipment.enabled = false;
        backpack.enabled = false;
        container.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        staminaBar.value = player.stamina / 100f;
        healthBar.value = player.health / 100f;
        foodBar.value = player.food / 100f;
        waterBar.value = player.water / 100f;

        timeOfDay.GetComponentInChildren<Text>().text = System.DateTime.Now.ToString("hh:mm tt");

        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryOpen = !inventoryOpen;
            inventoryPanel.enabled = inventoryOpen;
            statsUi.enabled = !inventoryOpen;
            equipment.enabled = inventoryOpen;
            backpack.enabled = inventoryOpen;
            if (containerOpen)
            {
                containerOpen = false;
                container.enabled = false;
            }
        }

        
        
    }

    IEnumerator clearPickupText()
    {
        yield return new WaitForSeconds(4f);
        text.text = "";
        pickupAmount = 0f;
    }
        

    private float pickupAmount;

    public void triggerEvent(Harvest harvest)
    {
        pickupAmount += harvest.harvestAmount;

        StopCoroutine("clearPickupText");
        StringBuilder builder = new StringBuilder();
        builder.Append("You have recieved ");
        //TODO: Remove field and replace with better calculation of how many items to display on pickup
        builder.Append(pickupAmount);
        //TODO: Remove field and replace with better calculation of how many items to display on pickup
        builder.Append(" ");
        builder.Append(harvest.matType.ToString());
        text.text = builder.ToString();

        StartCoroutine(clearPickupText());
    }

}
