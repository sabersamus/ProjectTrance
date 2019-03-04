using System.Collections;
using System.Collections.Generic;
using System;
using System.Data;
using UnityEngine;
using Mono.Data.SqliteClient;

public class ItemDAO 
{
    private IDbConnection connection;

    public ItemDAO(IDbConnection dbConnection)
    {
        connection = dbConnection;
    }

    public Item getItemByName(string name)
    {
        foreach(Item item in getItems())
        {
            if (item.itemName.Equals(name))
            {
                return item;
            }
        }
        return null;
    }

    public List<Item> getItems()
    {
        List<Item> items = new List<Item>();
        
        string query = "SELECT * FROM item";
        IDbCommand command = connection.CreateCommand();

        command.CommandText = query;

        IDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Item item = new Item();

            int id = reader.GetInt32(0);
            string itemName = reader.GetString(1);
            int maxStackSize = reader.GetInt32(2);
            string description = reader.GetString(3);
            bool isTool = reader.GetBoolean(4);
            bool isCraftable = reader.GetBoolean(6);

            ItemType itemType = (ItemType)Enum.Parse(typeof(ItemType), itemName.ToUpper().Replace(' ', '_'));

            item.itemType = itemType;
            item.itemName = itemName;
            item.maxStackSize = maxStackSize;
            item.description = description;
            item.isTool = isTool;
            item.isCraftable = isCraftable;

            items.Add(item);

        }

        reader.Close();


        return items;
    }

    public Item getItemByType(ItemType itemType)
    {
        foreach(Item item in getItems())
        {
            if (item.itemType == itemType)
            {
                return item;
            }
        }
        return null;
    }

    public List<CraftableItem> getCraftableItems()
    {
        List<CraftableItem> items = new List<CraftableItem>();

        string query = "SELECT * FROM item where isCraftable = 1";
        IDbCommand command = connection.CreateCommand();

        command.CommandText = query;

        IDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            CraftableItem item = new CraftableItem();

            int id = reader.GetInt32(0);
            string itemName = reader.GetString(1);
            int maxStackSize = reader.GetInt32(2);
            string description = reader.GetString(3);

            ItemType itemType = (ItemType)Enum.Parse(typeof(ItemType), itemName.ToUpper().Replace(' ', '_'));

            item.itemType = itemType;
            item.itemName = itemName;
            item.maxStackSize = maxStackSize;
            item.description = description;
            setRecipe(item);

            items.Add(item);

        }

        reader.Close();


        return items;
    }

    private void setRecipe(CraftableItem item)
    {
        string query = $"SELECT * FROM item_recipes WHERE outputItem = '{item.itemType.ToString()}'";

        IDbCommand command = connection.CreateCommand();
        command.CommandText = query;

        IDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            string outputItem = reader.GetString(0);
            string requiredItems = reader.GetString(1);
            int outputAmount = reader.GetInt32(2);


            Recipe recipe = new Recipe();
            List<ItemStack> recipeInput = new List<ItemStack>();

            if (requiredItems.Contains(","))
            {
                string[] array1 = requiredItems.Split(',');
                Dictionary<string, int> itemsToParse = new Dictionary<string, int>();
                foreach(string itemParser in array1)
                {
                    Debug.Log("ItemParser: " + itemParser);
                    //here we split by : to get item name and item amount
                    string[] array2 = itemParser.Split(':');
                    string itemName = array2[0];
                    int amount = int.Parse(array2[1]);
                    Debug.Log(itemName + ":" + amount);
                    itemsToParse.Add(itemName, amount);
                }

                foreach(string itemName in itemsToParse.Keys)
                {
                    Debug.Log(itemName);
                    Item parsedItem = getItemByName(itemName);
                    Debug.Log(parsedItem.itemName);
                    int amount = itemsToParse[itemName];

                    ItemStack itemStack = new ItemStack(parsedItem, amount);

                    recipeInput.Add(itemStack);
                }
            }
            else
            {

                string[] array2 = requiredItems.Split(':');
                string itemName = array2[0];
                int amount = int.Parse(array2[1]);
                Debug.Log(itemName + ":" + amount);
            }




            recipe.Input = recipeInput;
            recipe.Output = getCraftableItemByName(outputItem);
            recipe.outputAmount = outputAmount;
            item.recipe = recipe;

        }

        return;
    }

    public CraftableItem getCraftableItemByType(ItemType itemType)
    {
        foreach(CraftableItem item in getCraftableItems())
        {
            if (item.itemType == itemType)
            {
                return item;
            }
        }
        return null;
    }

    public CraftableItem getCraftableItemByName(string name)
    {
        foreach (CraftableItem item in getCraftableItems())
        {
            if (item.itemName == name)
            {
                return item;
            }
        }
        return null;
    }


    public List<ToolItem> getToolItems()
    {
        List<ToolItem> items = new List<ToolItem>();

        string query = "SELECT * FROM item WHERE isTool = 1";
        IDbCommand command = connection.CreateCommand();

        command.CommandText = query;

        IDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            ToolItem toolItem = new ToolItem();

            int id = reader.GetInt32(0);
            string itemName = reader.GetString(1);
            int maxStackSize = reader.GetInt32(2);
            string description = reader.GetString(3);
            //column 4 is isTool
            string _toolType = reader.GetString(5);

            ItemType itemType = (ItemType)Enum.Parse(typeof(ItemType), itemName.ToUpper().Replace(' ', '_'));
            ToolType toolType = (ToolType)Enum.Parse(typeof(ToolType), _toolType.ToUpper());

            toolItem.itemType = itemType;
            toolItem.itemName = itemName;
            toolItem.maxStackSize = maxStackSize;
            toolItem.description = description;
            toolItem.toolType = toolType;

            items.Add(toolItem);

        }

        reader.Close();

        return items;
    }

    public ToolItem getToolItemByType(ItemType itemType)
    {
        foreach (ToolItem toolItem in getToolItems())
        {
            if (toolItem.itemType == itemType)
            {
                return toolItem;
            }
        }
        return null;
    }
}
