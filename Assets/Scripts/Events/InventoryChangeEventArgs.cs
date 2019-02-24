using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryChangeEventArgs : System.EventArgs
{

    public readonly float eventId;
    public readonly Inventory inventory;
    public readonly InventoryChangeType changeType;
    public ItemStack itemStack;

    public InventoryChangeEventArgs(Inventory _inventory, InventoryChangeType _changeType, ItemStack _itemStack)
    {
        inventory = _inventory;
        changeType = _changeType;
        itemStack = _itemStack;
        eventId = Random.Range(0, 999999);
    }


    public enum InventoryChangeType
    {
        ADD,
        REMOVE
    }
}
