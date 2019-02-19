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
    public Text text;
    public Slider healthBar;
    public Slider staminaBar;
    public Slider foodBar;
    public Slider waterBar;

    [HideInInspector]
    public Slot[] inventorySlots;
    private static SpriteRenderer inventorySpriteRender;

    public GameObject inventorySlotsHolder;
    


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
        inventoryOpen = false;
        inventoryPanel.enabled = false;
        equipment.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        staminaBar.value = player.stamina / 100f;
        healthBar.value = player.health / 100f;
        foodBar.value = player.food / 100f;
        waterBar.value = player.water / 100f;

        timeOfDay.GetComponentInChildren<Text>().text = System.DateTime.Now.ToString("hh:mm tt");

        if (Input.GetButtonDown("I"))
        {
            inventoryOpen = !inventoryOpen;
            inventoryPanel.enabled = inventoryOpen;
            statsUi.enabled = !inventoryOpen;
            hotbar.enabled = !inventoryOpen;
            equipment.enabled = inventoryOpen;
        }

        
        
    }

    IEnumerator clearPickupText()
    {
        yield return new WaitForSeconds(4f);
        text.text = "";
        pickupAmount = 0f;
    }
        

    public void disableSlot(int slot)
    {
        inventorySpriteRender = inventorySlots[slot].GetComponent<SpriteRenderer>();
        inventorySpriteRender.enabled = false;
    }

    public void enableSlot(int slot, ItemStack item, Sprite sprite)
    {
        SpriteRenderer spriteRenderer = inventorySlots[slot].GetComponent<SpriteRenderer>();
        inventorySlots[slot].item = item;
        spriteRenderer.sprite = sprite;
        spriteRenderer.enabled = true;
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
        builder.Append(harvest.matType.ToString().ToLower());
        text.text = builder.ToString();

        StartCoroutine(clearPickupText());
    }

}
