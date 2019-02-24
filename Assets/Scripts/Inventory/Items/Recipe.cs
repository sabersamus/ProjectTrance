using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "Recipe", menuName = "Recipe/Recipe", order = 2)]
public class Recipe : ScriptableObject
{
    [SerializeField]
    public GameItem[] requiredItems;
    [SerializeField]
    public int[] requiredAmount;
    [SerializeField]
    public CraftableItem outputItem;
    [SerializeField]
    public int outputAmount;

    public Dictionary<GameItem, int> input
    {
        get
        {
            Dictionary<GameItem, int> list = new Dictionary<GameItem, int>();
            for (int i = 0; i < requiredItems.Length; i++)
            {
                list.Add(requiredItems[i], requiredAmount[i]);
            }
            return list;
        }
    }

    public ItemStack itemStack
    {
        get
        {
            return new ItemStack(outputItem, outputAmount);
        }
    }

}
