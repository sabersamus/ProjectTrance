using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="GameItem", menuName = "GameItem/GameItem", order = 0)]
[System.Serializable]
public class GameItem : ScriptableObject
{
    public int itemId;
    public string itemName;
    public bool isStackable;
    public int maxStackSize;
}
