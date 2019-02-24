using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    private static Dictionary<CraftableItem, Recipe> recipes;

    void Awake()
    {
        recipes = new Dictionary<CraftableItem, Recipe>();
        foreach (Recipe recipe in Resources.FindObjectsOfTypeAll<Recipe>() as Recipe[])
        {
            recipes.Add(recipe.outputItem, recipe);
        }
    }

    public static Recipe getRecipeByItem(CraftableItem item)
    {
        return recipes[item];
    }
}
