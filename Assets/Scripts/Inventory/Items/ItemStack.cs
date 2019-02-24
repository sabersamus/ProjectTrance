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
    public GameItem gameItem;
    [SerializeField]
    public int stackSize;


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

    #region overrides

    public override bool Equals(object obj)
    {
        if (!(obj is ItemStack)) return false;

        ItemStack _itemStack = (ItemStack)obj;

        return _itemStack.gameItem == gameItem && _itemStack.stackSize == stackSize;

    }

    public override int GetHashCode()
    {
        var hashCode = -551140938;
        hashCode = hashCode * -1521134295 + EqualityComparer<GameItem>.Default.GetHashCode(gameItem);
        hashCode = hashCode * -1521134295 + stackSize.GetHashCode();
        return hashCode;
    }

    #endregion
}
