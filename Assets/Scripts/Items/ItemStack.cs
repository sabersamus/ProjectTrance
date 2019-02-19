using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ItemStack
{

    public int stackSize;
    public Resource itemType;
    public string name;
    public Sprite sprite;

    public ItemStack(Resource item, int _stackSize)
    {
        int maxStackSize = item.maxStackSize;
        itemType = item;
        if(_stackSize > maxStackSize)
        {
            _stackSize = maxStackSize;
        }
        stackSize = _stackSize;
    }

    public bool isSimilar(ItemStack item)
    {
        if (item == null) return false;
        if (item == this) return true;
        if (item.itemType == this.itemType) return true;
        return false;
    }
    
}
