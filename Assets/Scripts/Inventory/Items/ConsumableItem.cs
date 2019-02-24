using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName ="ConsumableItem", menuName="GameItem/ConsumableItem", order =2)]
public class ConsumableItem : GameItem
{
    [Serializable]
    public enum ConsumableType
    {
        FOOD,
        WATER,
        MEDICAL
    }

    [SerializeField]
    public ConsumableType consumableType;
    [SerializeField]
    public ConsumableEffect[] effects;

}
