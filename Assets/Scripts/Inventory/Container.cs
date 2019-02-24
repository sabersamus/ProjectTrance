using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour, IInventoryHolder
{
    [SerializeField]
    private Inventory inventory;

    [SerializeField, Range(6, 32)]
    public int containerSize;

    public Inventory getInventory()
    {
        return inventory;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(inventory == null)
        {
            inventory = new Inventory(this, containerSize);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
