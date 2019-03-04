using System.Collections;
using System.Collections.Generic;

public class Recipe 
{
    public List<ItemStack> Input
    {
        get;
        set;
    }

    public CraftableItem Output
    {
        get;
        set;
    }

    public int outputAmount
    {
        get;
        set;
    }
}
