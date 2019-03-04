using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour, IInventoryHolder
{
    private Inventory inventory;

    [SerializeField, Range(6, 32)]
    public int containerSize;
    [SerializeField]
    private ItemStack[] predefinedContents;

    public Inventory getInventory()
    {
        return inventory;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (inventory == null)
        {
            inventory = new Inventory(this, containerSize);
        }
        if(predefinedContents != null)
        {
            Debug.Log("Predetermined contents found. Setting contents");
            foreach (ItemStack itemStack in predefinedContents)
            {
                inventory.addItem(itemStack);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
