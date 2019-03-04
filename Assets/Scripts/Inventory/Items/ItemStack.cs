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
    public Item item;
    [SerializeField]
    public int stackSize;


    public ItemStack(Item _item, int _stackSize)
    {
        if (!_item.isStackable || _item is ToolItem)
        {
            _stackSize = 1;
        }
        item = _item;
        stackSize = _stackSize;
    }

    public ItemStack(ItemType itemType, int _stackSize)
    {
        Item _item = Item.fromType(itemType);

        if (!_item.isStackable || _item is ToolItem)
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

    #region overrides

    public override bool Equals(object obj)
    {
        if (!(obj is ItemStack)) return false;

        ItemStack _itemStack = (ItemStack)obj;

        return _itemStack.item == item && _itemStack.stackSize == stackSize;

    }

    public override int GetHashCode()
    {
        var hashCode = -551140938;
        hashCode = hashCode * -1521134295 + EqualityComparer<Item>.Default.GetHashCode(item);
        hashCode = hashCode * -1521134295 + stackSize.GetHashCode();
        return hashCode;
    }

    #endregion
}
