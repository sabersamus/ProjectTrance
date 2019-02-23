using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="EquipableItem", menuName ="GameItem/Equipable", order = 1)]
public class EquipableItem : GameItem
{

    public EquipmentSlot equipmentSlot;
    public int ballisticDefenceBonus;


}
