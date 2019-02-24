using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : Inventory, ICraftingInventory
{
    public PlayerInventory(IInventoryHolder inventoryHolder, int _maxSize) : base(inventoryHolder, _maxSize)
    {

    }

    public bool canCraft(CraftableItem output)
    {
        bool craftable = true;
        List<ItemStack> requiredItems = output.recipe.requiredMaterials;
        for(int i = 0; i < requiredItems.Count; i++)
        {
            if (requiredItems[i] == null) continue;

            if (!containsAtleast(requiredItems[i].gameItem, requiredItems[i].stackSize))
            {
                craftable = false;
            }
        }
        return craftable;
    }

    public bool craftItem(Recipe input)
    {
        if (!canCraft(input.output))
        {
            return false;
       }

        ItemStack output = new ItemStack(input.output, input.outputAmount);

        foreach(ItemStack inputItem in input.requiredMaterials)
        {
            remove(inputItem.gameItem, inputItem.stackSize);
        }
        addItem(output);
        return true;
    }

    public bool craftItem(CraftableItem expectedOutput)
    {
        return craftItem(expectedOutput.recipe);
    }
}
