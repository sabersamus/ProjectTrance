using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Item
{
    public string itemName
    {
        get;
        set;
    }

    [SerializeField]
    public ItemType itemType;

    public int maxStackSize
    {
        get;
        set;
    }

    public bool isTool
    {
        get;
        set;
    }

    public bool isCraftable
    {
        get;
        set;
    }

    public bool isStackable
    {
        get
        {
            return maxStackSize > 1;
        }
    }

    public string description
    {
        get;
        set;
    }

    public override string ToString()
    {
        return $"{itemName}: {description}";
    }

    public static Item fromType(ItemType itemType)
    {
        return DatabaseManager.getDBContext().getItemDAO().getItemByType(itemType);
    }
}
