using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICraftingInventory
{
    bool craftItem(Recipe input);
    bool craftItem(CraftableItem expectedOutput);
    bool canCraft(CraftableItem output);
}
