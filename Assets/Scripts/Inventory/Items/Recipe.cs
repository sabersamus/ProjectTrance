using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "Recipe", menuName = "Recipe/Recipe", order = 2)]
public class Recipe : ScriptableObject
{

    [SerializeField]
    public List<ItemStack> requiredMaterials;
    [SerializeField]
    public CraftableItem output;
    [SerializeField]
    public int outputAmount;

}
