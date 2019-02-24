using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName="CraftableItem", menuName ="GameItem/CraftableItem", order= 3)]
public class CraftableItem : GameItem
{
    [SerializeField]
    public Recipe recipe;
    [SerializeField]
    public int outputAmount;
}
