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
        foreach(GameItem gameItem in output.recipe.requiredItems)
        {
            if(!contains(gameItem, output.recipe.itemStack.stackSize))
            {
                craftable = false;
            }
        }
        return craftable;
    }

    public bool craftItem(Recipe input)
    {
        if (!canCraft(input.outputItem))
        {
            return false;
        }

        ItemStack output = input.itemStack;
        foreach(GameItem inputItem in input.input.Keys)
        {
            Debug.Log("Removing " + input.input[inputItem] + " of " + inputItem.itemName);
            remove(inputItem, input.input[inputItem]);
        }
        addItem(output);

        return true;
    }

    public bool craftItem(CraftableItem expectedOutput)
    {
        return craftItem(expectedOutput.recipe);
    }
}
