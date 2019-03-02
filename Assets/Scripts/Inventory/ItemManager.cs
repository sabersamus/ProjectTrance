﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private static Dictionary<int, GameItem> gameItems;

    void Awake()
    {
        gameItems = new Dictionary<int, GameItem>();

        foreach (GameItem gameItem in Resources.FindObjectsOfTypeAll<GameItem>() as GameItem[])
        {
            gameItems.Add(gameItem.itemId, gameItem);
        }

    }

    public static GameItem getGameItemById(int id)
    {
        return gameItems[id];
    }

    public static GameItem getGameItemByName(string name)
    {
        foreach(GameItem gameItem in gameItems.Values)
        {
            if (gameItem.name.Equals(name)) return gameItem;
        }
        return null;
    }

    public static GameItem getGameItemByType(ItemType _type)
    {
        foreach(GameItem gameItem in gameItems.Values)
        {
            string itemName = gameItem.itemName.ToString().ToLower().Replace("_", " ");
            string typeName = _type.ToString().ToLower().Replace("_", " ");
            if (itemName.Equals(typeName))
            {
                return gameItem;
            }
        }
        return null;
    }

}
