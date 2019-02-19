using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Specialized;
using System;

public class Inventory
{

    [SerializeField]
    public int size;
    [SerializeField]
    public int maxSize;
    //public Dictionary<int, ItemStack> items;
    [SerializeField]
    public ItemStack[] items;

    public GameObject slotsHolder;
    public PlayerUI playerUi;

    public Slot[] slots;

    public int currentSize()
    {
        int count = 0;
        for(int i = 0; i < items.Length; i++)
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
    public bool addItem(ItemStack item)
    {   
        if(maxSize < currentSize() + 1)
        {
            Debug.Log("Inventory is full");
            return false;
        }

        //if we have a partial stack
        if (firstPartial(item) != -1)
        {
            Debug.Log("We have a partial stack.... filling");
            //first partial itemstack of the type 
            int _firstPartialID = firstPartial(item);
            ItemStack _item = getItemInSlot(_firstPartialID);

            int _size = _item.stackSize;
            int _inputSize = item.stackSize;

            int _overFlow = 0;

            if (_size + _inputSize > _item.itemType.maxStackSize)
            {
                _overFlow = _size + _inputSize - _item.itemType.maxStackSize;
            }

            //if there is no overflow
            if (_overFlow != 0)
            {
                Debug.Log("No overflow, finishing up");
                ItemStack _fp = new ItemStack(_item.itemType, _size + _inputSize);
                items[_firstPartialID] = _fp;
                slots[_firstPartialID].item = _fp;
                return true;
            }
            else
            {
                Debug.Log("We have overflow... checking");
                //If we DO have overflow

                /*
                 * Step 1: Set the first partial stack to maxStackSize
                 * Step 2: Check for the next partial stack, if any
                 * Step 2.5: If we dont have a next partial, we will look for an empty
                 */

                //Step 1
                items[_firstPartialID] = new ItemStack(_item.itemType, _item.itemType.maxStackSize);

                //Step 2
                int _nextPartialID = firstPartial(item);

                //if there is no next partial stack
                if (_nextPartialID == -1)
                {
                    if (currentSize() + 1 > maxSize)
                    {
                        Debug.Log("Inventory is full, overflow not kept");
                        //Maybe throw exception?
                    }
                    else
                    {
                        //Step 2.5: No next partial, lets look for an empty
                        int emptyId = firstEmpty();

                        if(emptyId == -1)
                        {
                            Debug.Log("Your logic is flawed, this shouldnt be possible");
                            return false;
                        }
                        else
                        {
                            //We found an empty slot, so we put the overflow in the empty slot 
                            //and finish up
                            ItemStack toEmpty = new ItemStack(item.itemType, _overFlow);
                            setItem(emptyId, toEmpty);
                            slots[emptyId].item = toEmpty;
                            return true;
                        }
                    }
                }
                else
                {
                    //We do have a next partial stack
                    int _nextPartialSize = getItemInSlot(_nextPartialID).stackSize;
                    //We add the overflow to the stacksize of our partial
                    setItem(_nextPartialID, new ItemStack(_item.itemType, _nextPartialSize + _overFlow));
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
                Debug.Log("Inventory full, no room for item");
                return false;
            }
            else
            {
                Debug.Log("Empty slot found, filling");
                //We have found an empty slot
                int id = firstEmpty();
                //well just set the empty slot to be the itemstack
                setItem(id, item);
                slots[id].item = item;
                Debug.Log("Setting slot " + id + " to " + item.itemType.matType);
                playerUi.enableSlot(id, item, item.sprite);
                return true;
            }
        }
        return false;
    }

    public void removeItem(ItemStack item)
    {
        //TODO: Remove item from inventory
    }

    public int firstPartial(ItemStack item)
    {
        ItemStack[] inventory = items;

        if (item == null)
        {
            return -1;
        }

        for(int i = 0; i< inventory.Length; i++)
        {
            ItemStack _item = inventory[i];
            if(_item != null && _item.stackSize 
                < _item.itemType.maxStackSize && _item.isSimilar(item))
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


    public void setItem(int slot, ItemStack item)
    {
        if(slot > maxSize || slot < 0)
        {
            Debug.Log("Invalid slot key");
            return;
        }
        items[slot] = item;
        Debug.Log(slots[slot]);
        slots[slot].item = item;
        slots[slot].sprite = item.sprite;
        playerUi.enableSlot(slot, item, item.sprite);

    }

    public int findFirstSlot(ItemStack item)
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

    public ItemStack getItemInSlot(int key)
    {
        return items[key];
    }



    
}
