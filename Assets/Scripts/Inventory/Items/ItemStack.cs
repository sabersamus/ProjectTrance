using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// An ItemStack represents a stack of GameItems
/// </summary>
[Serializable]
public class ItemStack
{
    public GameItem item
    {
        get;
        set;
    }

    public int stackSize
    {
        get;
        set;
    }


    public ItemStack(GameItem _item, int _stackSize)
    {
        if (!_item.isStackable || _item is EquipableItem)
        {
            _stackSize = 1;
        }
        item = _item;
        stackSize = _stackSize;
    }

    public bool isSimilar(ItemStack _item)
    {
        if (_item == null) return false;
        if (_item == this) return true;
        if (this.item == _item.item) return true;
        return false;
    }
    
}
