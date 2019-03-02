﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Specialized;
using System;

[Serializable]
public class Inventory
{

    [SerializeField]
    public int size;
    [SerializeField]
    public int maxSize;
    //public Dictionary<int, ItemStack> items;
    [SerializeField]
    public ItemStack[] items;

    private IInventoryHolder inventoryHolder;

    public IInventoryHolder GetInventoryHolder()
    {
        return inventoryHolder;
    }

    public Inventory(IInventoryHolder inventoryHolder, int _maxSize)
    {
        maxSize = _maxSize;
        this.inventoryHolder = inventoryHolder;

        items = new ItemStack[maxSize];
    }

    #region Events

    public static event EventHandler<InventoryChangeEventArgs> InventoryChanged;

    #endregion




    public int currentSize()
    {
        int count = 0;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                continue;
            }
            count++;
        }

        return count;
    }


    //I believe this method is finished
    public bool addItem(ItemStack _itemStack)
    {
        if (maxSize < currentSize() + 1)
        {
            return false;
        }


        if(InventoryChanged != null)
        {
            InventoryChanged(this, new InventoryChangeEventArgs(this, InventoryChangeEventArgs.InventoryChangeType.ADD, _itemStack));
        }

        //if we have a partial stack
        if (firstPartial(_itemStack) != -1)
        {
            //first partial itemstack of the type 
            int _firstPartialID = firstPartial(_itemStack);

            ItemStack _firstPartial = itemInSlot(_firstPartialID);

            int _firstPartialSize = _firstPartial.stackSize;
            int _inputSize = _itemStack.stackSize;

            int _overFlow = 0;

            if (_firstPartialSize + _inputSize > _firstPartial.gameItem.maxStackSize)
            {
                _overFlow = (_firstPartialSize + _inputSize) - _firstPartial.gameItem.maxStackSize;
            }

            //if there is no overflow
            if (_overFlow == 0)
            {
                ItemStack _fp = new ItemStack(_firstPartial.gameItem, _firstPartialSize + _inputSize);
                items[_firstPartialID] = _fp;
                return true;
            }
            else
            {
                //If we DO have overflow

                /*
                 * Step 1: Set the first partial stack to maxStackSize
                 * Step 2: Check for the next partial stack, if any
                 * Step 2.5: If we dont have a next partial, we will look for an empty
                 */

                //Step 1
                items[_firstPartialID] = new ItemStack(_firstPartial.gameItem, _firstPartial.gameItem.maxStackSize);

                //Step 2
                int _nextPartialID = firstPartial(_itemStack);

                //if there is no next partial stack
                if (_nextPartialID == -1)
                {
                    if (currentSize() + 1 > maxSize)
                    {
                        Debug.Log("Inventory is full, overflow not kept");
                        //TODO: throw item out
                        return false;
                    }
                    else
                    {
                        //Step 2.5: No next partial, lets look for an empty
                        int emptyId = firstEmpty();

                        if (emptyId == -1)
                        {
                            //We shouldnt have gotten to this point, because we already checked
                            //if we have room. by doing this, we should have an empty slot.
                            Debug.Log("Your logic is flawed, this shouldnt be possible");
                            return false;
                        }
                        else
                        {
                            //We found an empty slot, so we put the overflow in the empty slot 
                            //and finish up
                            ItemStack toEmpty = new ItemStack(_itemStack.gameItem, _overFlow);
                            setItem(emptyId, toEmpty);
                            return true;
                        }
                    }
                }
                else
                {
                    //We do have a next partial stack
                    int _nextPartialSize = itemInSlot(_nextPartialID).stackSize;
                    //We add the overflow to the stacksize of our partial
                    setItem(_nextPartialID, new ItemStack(_itemStack.gameItem, _nextPartialSize + _overFlow));
                    return true;
                }
            }
        }
        else //If we do not have a partial stack
        {

            //Step one see if we have an empty

            if (firstEmpty() == -1)
            {
                //We have no empty
                return false;
            }
            else
            {
                //We have found an empty slot
                int firstEmptyId = firstEmpty();
                //well just set the empty slot to be the itemstack
                setItem(firstEmptyId, _itemStack);
                return true;

            }
        }

    }

    /// <summary>
    /// Removes the ItemStack from the inventory.
    /// </summary>
    /// <param name="itemStack">The itemstack to be removed</param>
    public void removeItem(ItemStack itemStack)
    {
        if (itemStack == null)
        {
            Debug.Log("Itemstack can not be null");
            return;
        }

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == itemStack)
            {
                items[i] = null;
                break;
            }
        }
    }

    /// <summary>
    /// Removes <paramref name="amount"/> of <paramref name="itemType"/> from the 
    /// inventory.
    /// </summary>
    /// <param name="itemType">The type of item to remove</param>
    /// <param name="amount">The amount to remove</param>
    public bool remove(GameItem itemType, int amount)
    {
        if (itemType == null || amount <= 0)
        {
            Debug.Log("Illegal arguments");
            return false;
        }

        if (!contains(itemType))
        {
            Debug.Log("This inventory does not contain " + itemType.itemName);
            return false;
        }

        int overFlow = 0;

        if(amount > itemType.maxStackSize)
        {
            overFlow = amount - itemType.maxStackSize;
        }

        //We are removing more than a full stack
        if(overFlow != 0)
        {

            //first we remove our first full stack.
            //then, we set up a loop to remove any next stack
            //until our overflow is 0

            setItem(first(itemType), null);

            while(overFlow < 0)
            {
                //step 1, get first stack.
                //step 2, remove items from stack
                int _first = first(itemType);
                if (_first == -1) break;

                int _firstStackSize = items[_first].stackSize;

                //if the amount we need to remove is exactly how much our stack is
                if (_firstStackSize == overFlow)
                {
                    setItem(_first, null);
                    //this will break us from our loop
                    overFlow = 0;
                }
                else
                {
                    //if the stack is not exactly the right amount
                    //which will be the usual case.

                    if (overFlow > _firstStackSize)
                    {
                        //if we need to remove more than the stack has

                        //step 1, clear our current stack.
                        //step 2, subtract from how much we need to remove
                        //step 3, let our loop continue its work.
                        overFlow -= _firstStackSize;
                        setItem(_first, null);
                    }
                    else
                    {
                        //if we need to remove less than the stack has

                        int leftOver = overFlow - _firstStackSize;
                        setItem(_first, new ItemStack(itemType, leftOver));
                        overFlow = 0;
                        break;
                    }
                }
            }
            return true;

        }
        else
        {
            //we are removing less than or equal to a full stack
            int toRemove = amount;
            while(toRemove > 0)
            {
                //step 1, get first stack.
                //step 2, remove items from stack
                int _first = first(itemType);
                if (_first == -1) break;

                int _firstStackSize = items[_first].stackSize;

                //if the amount we need to remove is exactly how much our stack is
                if(_firstStackSize == toRemove)
                {
                    setItem(_first, null);
                    //this will break us from our loop
                    toRemove = 0;
                }
                else
                {
                    //if the stack is not exactly the right amount
                    //which will be the usual case.

                    if(toRemove > _firstStackSize)
                    {
                        //if we need to remove more than the stack has

                        //step 1, clear our current stack.
                        //step 2, subtract from how much we need to remove
                        //step 3, let our loop continue its work.
                        toRemove -= _firstStackSize;
                        setItem(_first, null);
                    }
                    else
                    {
                        //if we need to remove less than the stack has

                        int leftOver = _firstStackSize - amount;
                        setItem(_first, new ItemStack(itemType, leftOver));
                        toRemove = 0;
                        break;
                    }
                }
            }
            return true;
        }
    }

    public void removeAll(GameItem itemType)
    {

    }

    public void clear(int id)
    {
        setItem(id, null);
    }

    public void clear()
    {
        for (int i = 0; i < items.Length; i++)
        {
            clear(i);
        }
    }

    public bool contains(ItemStack itemStack)
    {
        if (itemStack == null) return false;
        foreach (ItemStack _itemStack in items)
        {
            if (_itemStack.Equals(itemStack)) return true;
        }
        return false;
    }

    public bool contains(GameItem itemType, int amount)
    {
        if (isEmpty()) return false;
        if (itemType == null || amount <= 0) return false;
        if (isEmpty()) return false;

        int count = 0;

        foreach(ItemStack _itemStack in items)
        {
            if (_itemStack == null) continue;
            if (_itemStack.gameItem == null || _itemStack.gameItem != itemType) continue;
            count += _itemStack.stackSize;
        }

        if (count == 0) return false;

        return count >= amount;
    }

    public bool contains(GameItem itemType)
    {
        if (isEmpty()) return false;
        if (itemType == null) return false;
        bool contains = false;

        foreach(ItemStack item in items)
        {
            if (item == null) continue;
            if (item.gameItem == itemType)
            {
                contains = true;
                break;
            }
        }

        return contains;
    }

    public bool containsAtleast(GameItem item, int amount)
    {
        if (isEmpty()) return false;
        int currentAmount = 0;
        foreach(ItemStack itemStack in items)
        {
            if (itemStack == null) continue;
            if (itemStack.gameItem != item) continue;
            currentAmount += itemStack.stackSize;
        }

        return currentAmount >= amount;
    }

    public bool isEmpty()
    {
        if (items == null) return true;
        if (items.Length == 0)
        {
            return true;
        }

        //assume empty unless we find something
        bool empty = true;
        foreach (ItemStack itemStack in items)
        {
            if(itemStack != null)
            {
                empty = false;
                //lets get out as soon as we find one item
                break;
            }
        }


        return empty;
    }
    

    public int firstPartial(ItemStack itemStack)
    {
        ItemStack[] inventory = items;

        if (itemStack == null)
        {
            return -1;
        }

        for(int i = 0; i< inventory.Length; i++)
        {
            ItemStack _itemStack = inventory[i];

            
            if(_itemStack != null && _itemStack.stackSize 
                < _itemStack.gameItem.maxStackSize && _itemStack.isSimilar(itemStack))
            {
                return i;
            }
        }

        return -1;
    }

    public int firstPartial(GameItem itemType)
    {
        if (itemType == null)
        {
            return -1;
        }

        for (int i = 0; i < items.Length; i++)
        {
            GameItem _itemType = items[i].gameItem;


            if (_itemType != null && items[i].stackSize
                < _itemType.maxStackSize && items[i].gameItem == itemType)
            {
                return i;
            }
        }

        return -1;
    }

    public int firstEmpty()
    {
        ItemStack[] _items = items;
        if (items.Length == 0) return 0;
        for (int i = 0; i < _items.Length; i++)
        {
            if(_items[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    public int firstFull(GameItem itemType)
    {
        ItemStack[] _items = items;
        if (items.Length == 0) return -1;
        for(int i = 0; i < _items.Length; i++)
        {
            if(_items[i].gameItem == itemType && _items[i].stackSize == itemType.maxStackSize)
            {
                return i;
            }
        }
        return -1;
    }

    public void setItem(int slot, ItemStack item)
    {
        if(slot > maxSize || slot < 0)
        {
            Debug.Log("Invalid slot key");
            return;
        }
        items[slot] = item;
    }

    public int first(ItemStack item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
                continue;

            if (items[i].Equals(item) || items[i].isSimilar(item))
            {
                return i;
            }
        }
        return -1;
    }

    public int first(GameItem itemType)
    {
        for(int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
                continue;
            if(items[i].gameItem == itemType)
            {
                return i;
            }
        }
        return -1;
    }

    public ItemStack itemInSlot(int key)
    {
        return items[key];
    }



    
}
