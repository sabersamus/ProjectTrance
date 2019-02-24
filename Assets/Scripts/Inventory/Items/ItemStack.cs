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
    [SerializeField]
    public GameItem gameItem
    {
        get;
        set;
    }

    [SerializeField]
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
        gameItem = _item;
        stackSize = _stackSize;
    }

    public bool isSimilar(ItemStack _item)
    {
        if (_item == null) return false;
        if (_item == this) return true;
        if (this.gameItem == _item.gameItem) return true;
        return false;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is ItemStack)) return false;

        ItemStack _itemStack = (ItemStack)obj;

        return _itemStack.gameItem == gameItem && _itemStack.stackSize == stackSize;

    }

}
