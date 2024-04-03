using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkstationSystem : MonoBehaviour
{
    public string workstation;
    public InventoryLoader inputInventoryLoader;
    public InventoryLoader outputInventoryLoader;

    // the key string here is the class name of the item. NOT the displayname
    public Dictionary<string, RecipeClass> recipeOptions = new Dictionary<string, RecipeClass>();
    RecipeClass currentRecipe;
    public bool readyToBrew = false;

    public void CheckPotion()
    {
        LoadCSV();
        List<string> itemNames = new List<string>();
        bool validRecipe = false;

        foreach (KeyValuePair<string, ItemClass> entry in inputInventoryLoader.containerInv.totalInventory)
        {
            if(entry.Value.quantity > 0)
            {
                // Access the className property of the ItemClass and add it to the list
                itemNames.Add(entry.Key);
            }
        }

        foreach (KeyValuePair<string, RecipeClass> entry in recipeOptions)
        {
            // Check if either itemOne or itemTwo of the current RecipeClass matches any classNames in classNamesList
            if (itemNames.Contains(entry.Value.itemOne) && itemNames.Contains(entry.Value.itemTwo))
            {
                currentRecipe = entry.Value;
                validRecipe = true;
            }
        }

        if(validRecipe)
        {
            // we can actually make it here
            Debug.Log($"The items in the {workstation} match the recipe for {currentRecipe.displayName}");

            // check item quantities
            if(inputInventoryLoader.containerInv.totalInventory[itemNames[0]].quantity >= currentRecipe.itemOneQuantity)
            {
                if(inputInventoryLoader.containerInv.totalInventory[itemNames[1]].quantity >= currentRecipe.itemTwoQuantity)
                {
                    // we have a valid amount of items to make the potion
                    Debug.Log($"The items in the {workstation} have the correct quantity for {currentRecipe.displayName}");

                    // check the output isnt full
                    if(outputInventoryLoader.containerInv.inventory.AddItem(currentRecipe.className, 1))
                    {
                        // remove the item we added to test
                        outputInventoryLoader.containerInv.inventory.TakeItem(currentRecipe.className, 1);

                        outputInventoryLoader.RefreshInventories();
                        readyToBrew = true;
                    }
                }
                else{readyToBrew = false;}
            }
            else{readyToBrew = false;}
        }
        else
        {
            if(itemNames.Count == 0)
            {
                Debug.Log($"No items in the inventory of the {workstation}");
            }
            if(itemNames.Count == 1)
            {
                Debug.Log($"Items in the {workstation} do not match the items needed for any known recipe. The items are: {itemNames[0]}");
            }
            if(itemNames.Count == 2)
            {
                Debug.Log($"Items in the {workstation} do not match the items needed for any known recipe. The items are: {itemNames[0]}, {itemNames[1]}");
            }
            readyToBrew = false;
        }
    }

    public void BrewPotion()
    {
        if(readyToBrew)
        {
            // takes the items from the input inventory
            inputInventoryLoader.containerInv.inventory.TakeItem(currentRecipe.itemOne, currentRecipe.itemOneQuantity);

            if(currentRecipe.itemTwo != "")
            {
                inputInventoryLoader.containerInv.inventory.TakeItem(currentRecipe.itemTwo, currentRecipe.itemTwoQuantity);
            }

            // adds them to the ouput inventory
            outputInventoryLoader.containerInv.inventory.AddItem(currentRecipe.className, 1);

            inputInventoryLoader.RefreshInventories();
            outputInventoryLoader.RefreshInventories();
        }
    }

    public void SavePotions()
    {
        // do the save system save here
    }

    public void LoadPotions()
    {
        LoadCSV();
        // do the save system load here
    }

    public void LoadCSV()
    {
        // Load CSV file
        TextAsset textFile = Resources.Load<TextAsset>("workstationTable");

        // Split CSV into lines
        string[] lines = textFile.text.Split('\n');

        // Parse each line of the CSV
        for (int i = 2; i < lines.Length; i++)
        {
            string[] values = ParseCSVLine(lines[i]);

            // Check if all required values are present
            if (values.Length < 6)
            {
                Debug.LogError($"Incomplete data for line {i + 1}: {lines[i]}");
                continue;
            }

            if(values[3] == workstation)
            {
                // Attempt to parse values
                RecipeClass newRecipe = new RecipeClass();

                // Assign other values
                newRecipe.className = values[0];
                newRecipe.displayName = values[1];
                newRecipe.imageName = values[2];
                newRecipe.workstation = values[3];
                newRecipe.itemTwo = values[5];

                // if it has the asterisk its multiple items needed
                if(values[4].Contains("*"))
                {
                    string[] parts = values[4].Split('*');
                    string itemName = parts [0];
                    int quantity;

                    if (int.TryParse(parts[1], out quantity))
                    {
                        newRecipe.itemOneQuantity = quantity;
                        newRecipe.itemOne = itemName;
                    }
                    else
                    {
                        Debug.LogError($"Failed to add item: {values[4]} to the recipe for {values[0]}'s needed items due to an invalid quantity of: '{parts[1]}'");
                    }
                }
                else
                {
                    // doesent contain the asterisk so its a single item
                    newRecipe.itemOne = values[4];
                    newRecipe.itemOneQuantity = 1;
                }

                // if it has the asterisk its multiple items needed
                if(values[5].Contains("*"))
                {
                    string[] parts = values[5].Split('*');
                    string itemName = parts [0];
                    int quantity;

                    if (int.TryParse(parts[1], out quantity))
                    {
                        newRecipe.itemTwoQuantity = quantity;
                        newRecipe.itemTwo = itemName;
                    }
                    else
                    {
                        Debug.LogError($"Failed to add item: {values[5]} to the recipe for {values[0]}'s needed items due to an invalid quantity of: '{parts[1]}'");
                    }
                }
                else
                {
                    // doesent contain the asterisk so its a single item
                    newRecipe.itemTwo = values[5];
                    newRecipe.itemTwoQuantity = 1;
                }



                // Check if className exists in totalInventory
                if (recipeOptions.ContainsKey(values[0]))
                {
                    // Retrieve existing item
                    RecipeClass existingItem = recipeOptions[values[0]];

                    // Check if any property has changed (excluding stackSize)
                    if (
                        existingItem.displayName != newRecipe.displayName ||
                        existingItem.imageName != newRecipe.imageName ||
                        existingItem.workstation != newRecipe.workstation ||
                        existingItem.itemOne != newRecipe.itemOne ||
                        existingItem.itemTwo != newRecipe.itemTwo ||
                        existingItem.itemOneQuantity != newRecipe.itemOneQuantity ||
                        existingItem.itemTwoQuantity != newRecipe.itemTwoQuantity
                    )
                    {
                        // Update values
                        existingItem.displayName = newRecipe.displayName;
                        existingItem.imageName = newRecipe.imageName;
                        existingItem.workstation = newRecipe.workstation;
                        existingItem.itemOne = newRecipe.itemOne;
                        existingItem.itemTwo = newRecipe.itemTwo;
                        existingItem.itemOneQuantity = newRecipe.itemOneQuantity;
                        existingItem.itemTwoQuantity = newRecipe.itemTwoQuantity;

                        recipeOptions[values[0]] = existingItem;

                        Debug.Log($"Recipe {existingItem.className} updated in recipeOptions.");
                    }
                }
                else
                {
                    // Add newPotion to totalInventory
                    recipeOptions.Add(newRecipe.className, newRecipe);
                }
            }
        }

        // Debug output for verification
        foreach (var item in recipeOptions.Values)
        {
            //Debug.Log($"Item: {item.className}, Display Name: {item.displayName}, Image Name: {item.imageName}, Stack Size: {item.stackSize}, Rarity: {item.rarity}, Spawn Biome 1: {item.spawnBiome1}, Spawn Biome 2: {item.spawnBiome2}, Sell Price: {item.sellPrice}, Store Price: {item.storePrice}, Goblin Price: {item.goblinPrice}");
        }
    }


    public string[] ParseCSVLine(string csvLine)
    {
        List<string> fields = new List<string>();
        bool insideQuotes = false;
        string currentField = "";

        foreach (char c in csvLine)
        {
            if (c == ',' && !insideQuotes)
            {
                fields.Add(currentField);
                currentField = "";
            }
            else if (c == '"')
            {
                insideQuotes = !insideQuotes;
                // Optionally skip adding the quote to the field
            }
            else
            {
                currentField += c;
            }
        }

        fields.Add(currentField); // Add the last field
        return fields.ToArray();
    }    
}
