using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Inventory
{
    public int inventoryMaxLength = 10;

    // the key string here is the class name of the item. NOT the displayname
    public Dictionary<string, ItemClass> totalInventory = new Dictionary<string, ItemClass>();
    // the first value here is the class name of the item. NOT the displayname
    public List<string[]> formattedInventory = new List<string[]>();


    public List<string[]> GetInventory()
    {
        return formattedInventory;
    }

    public bool AddItem(string itemName, int quantity)
    {
        // Check if the item exists
        if(totalInventory.ContainsKey(itemName))
        {
            bool itemsAllAdded = false;
            
            // Check existing stacks for space
            foreach (string[] array in formattedInventory)
            {
                if(array[0] == itemName)
                {
                    int currentQuantity;
                    if (int.TryParse(array[1], out currentQuantity))
                    {
                        int stackSpace = totalInventory[itemName].stackSize - currentQuantity;
                        // If there's enough space in the stack, add the items
                        if(quantity <= stackSpace)
                        {
                            array[1] = (currentQuantity + quantity).ToString();
                            totalInventory[itemName].quantity += quantity;
                            itemsAllAdded = true;
                            Debug.Log($"InventoryStatus: Added {quantity} {itemName}(s) to an existing stack in the inventory");
                            return true;
                        }
                        else
                        {
                            // If there's not enough space in the stack, continue checking other stacks
                            quantity -= stackSpace;
                        }
                    }
                }
            }

            // If items couldn't be added to existing stacks, check if there's space for a new stack
            if(!itemsAllAdded && formattedInventory.Count < inventoryMaxLength)
            {
                addNewStack(itemName, quantity);
                Debug.Log($"InventoryStatus: Added {quantity} {itemName}(s) to the inventory as a new stack");
                return true;
            }
            else
            {
                Debug.LogError($"InventoryError: No inventory space left for {quantity} {itemName}(s)");
                return false;
            }
        }
        else
        {
            Debug.LogError($"InventoryError: Invalid item {itemName}");
            return false;
        }
    }

    public bool CanAddItems(string itemName, int quantity)
    {
        // Check if the item exists
        if(totalInventory.ContainsKey(itemName))
        {
            // Check existing stacks for space
            foreach (string[] array in formattedInventory)
            {
                if(array[0] == itemName)
                {
                    int currentQuantity;
                    if (int.TryParse(array[1], out currentQuantity))
                    {
                        int stackSpace = totalInventory[itemName].stackSize - currentQuantity;
                        // If there's enough space in the stack, return true
                        if(quantity <= stackSpace)
                        {
                            return true;
                        }
                        else
                        {
                            // If there's not enough space in the stack, continue checking other stacks
                            quantity -= stackSpace;
                        }
                    }
                }
            }

            // If items couldn't be added to existing stacks, check if there's space for a new stack
            if(formattedInventory.Count < inventoryMaxLength)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            // If the item doesn't exist, return false
            return false;
        }
    }

    // Method to add a new stack to the inventory
    void addNewStack(string itemName, int quantity)
    {
        string[] newItem = { itemName, quantity.ToString() };
        formattedInventory.Add(newItem);
        totalInventory[itemName].quantity += quantity;
    }
    
    public bool TakeItem(string itemName, int quantity)
    {
        // if the item exists
        if(totalInventory.ContainsKey(itemName))
        {
            // see how many we have
            if(totalInventory[itemName].quantity >= quantity)
            {
                // we have enough items to give
                // take it from the formatted inventory
                foreach (string[] array in formattedInventory)
                {
                    // if there is items and if we have any of them
                    if(array[0] == itemName)
                    {
                        // how many items have been removed from the inventory
                        int takingQuantity = 0;

                        // check to see if the stack is enough for what we need to give
                        if (int.TryParse(array[1], out takingQuantity))
                        {
                            if (takingQuantity >= quantity)
                            {
                                // Take the items
                                array[1] = (takingQuantity - quantity).ToString();

                                // Remove it from the total inventory
                                totalInventory[itemName].quantity -= quantity;

                                quantity = 0;

                                if(array[1] == "0")
                                {
                                    formattedInventory.Remove(array);
                                }

                                Debug.Log($"InventoryStatus: Removed {quantity} {itemName}s from the inventory");
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                Debug.LogError($"InventoryError: Inventory does not contain {quantity} {itemName}s so they cannot be taken");
                return false;
            }
            return true;
        }
        else
        {
            Debug.LogError($"InventoryError: Invalid item {itemName}");
        }
        return false;

        // get the inventory and see if there is any of the item to take, if there is -1 of that item and return true. This method will be used primarily by give item
    }

    public bool IsPotion(string itemName)
    {
        // if the item exists
        if(totalInventory.ContainsKey(itemName))
        {
            if(totalInventory[itemName].isPotion == "TRUE")
            {
                return true;
            }
            else
            {
                Debug.Log("item name is + " + itemName + " also the bool is: '" + totalInventory[itemName].isPotion + "sd'");
                Debug.Log($"Potion is: {itemName} and the bool is '{totalInventory[itemName].isPotion}'");
                return false;
            }
        }
        Debug.Log($"Total inventory does not contain key {itemName}");
        return false;
    }

    public bool Contains(string itemName)
    {
        // if the item exists
        if(totalInventory.ContainsKey(itemName))
        {
            if(totalInventory[itemName].quantity >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public string GetDisplayName(string itemName)
    {
        // if the item exists
        if(totalInventory.ContainsKey(itemName))
        {
            return totalInventory[itemName].displayName;
        }
        return "";
    }

    public void LoadCSV()
    {
        // // loop thru the csv, see if the className (column 0 (not rows 0 or 1)) exists in totalInventory as a key. If it doesent make a new instance of "ItemClass", add in the info from the csv and add it into totalInventory
        // ItemClass.className is column 0
        // ItemClass.displayName is column 1
        // ItemClass.imageName is column 2
        // ItemClass.stackSize is column 6
        // ItemClass.rarity is column 3
        // ItemClass.spawnBiome1 is column 4
        // ItemClass.spawnBiome2 is column 5
        // ItemClass.sellPrice is column 7
        // ItemClass.storePrice is column 8
        // ItemClass.goblinPrice is column 9

        // Load CSV file
        TextAsset textFile = Resources.Load<TextAsset>("itemTable");

        // Split CSV into lines
        string[] lines = textFile.text.Replace("\r", "").Split('\n');

        // Parse each line of the CSV
        for (int i = 2; i < lines.Length; i++)
        {
            string[] values = ParseCSVLine(lines[i]);

            // Check if all required values are present
            if (values.Length < 11)
            {
                Debug.LogError($"Incomplete data for line {i + 1}: {lines[i]}");
                continue;
            }

            // Attempt to parse values
            ItemClass newItem = new ItemClass();
            if (!int.TryParse(values[6], out newItem.stackSize))
            {
                Debug.LogError($"Failed to parse stackSize for line {i + 1}: {values[6]}");
                continue;
            }
            if (!int.TryParse(values[7], out newItem.sellPrice))
            {
                Debug.LogError($"Failed to parse sellPrice for line {i + 1}: {values[7]}");
                continue;
            }
            if (!int.TryParse(values[8], out newItem.storePrice))
            {
                Debug.LogError($"Failed to parse storePrice for line {i + 1}: {values[8]}");
                continue;
            }
            if (!int.TryParse(values[9], out newItem.goblinPrice))
            {
                Debug.LogError($"Failed to parse goblinPrice for line {i + 1}: {values[9]}");
                continue;
            }

            // Assign other values
            newItem.className = values[0];
            newItem.displayName = values[1];
            newItem.imageName = values[2];
            newItem.rarity = values[3];
            newItem.spawnBiome1 = values[4];
            newItem.spawnBiome2 = values[5];
            newItem.isPotion = values[10];

            // Check if className exists in totalInventory
            if (totalInventory.ContainsKey(values[0]))
            {
                // Retrieve existing item
                ItemClass existingItem = totalInventory[values[0]];

                // Check if any property has changed (excluding stackSize)
                if (existingItem.displayName != newItem.displayName ||
                    existingItem.imageName != newItem.imageName ||
                    existingItem.rarity != newItem.rarity ||
                    existingItem.spawnBiome1 != newItem.spawnBiome1 ||
                    existingItem.spawnBiome2 != newItem.spawnBiome2 ||
                    existingItem.sellPrice != newItem.sellPrice ||
                    existingItem.storePrice != newItem.storePrice ||
                    existingItem.stackSize != newItem.stackSize ||
                    existingItem.goblinPrice != newItem.goblinPrice ||
                    existingItem.isPotion != newItem.isPotion)
                {
                    // Update values
                    existingItem.displayName = newItem.displayName;
                    existingItem.imageName = newItem.imageName;
                    existingItem.rarity = newItem.rarity;
                    existingItem.spawnBiome1 = newItem.spawnBiome1;
                    existingItem.spawnBiome2 = newItem.spawnBiome2;
                    existingItem.sellPrice = newItem.sellPrice;
                    existingItem.storePrice = newItem.storePrice;
                    existingItem.stackSize = newItem.stackSize;
                    existingItem.goblinPrice = newItem.goblinPrice;
                    existingItem.isPotion = newItem.isPotion;

                    totalInventory[values[0]] = existingItem;

                    Debug.Log($"Item {existingItem.className} updated in inventory.");
                }
            }
            else
            {
                // Add newItem to totalInventory
                totalInventory.Add(newItem.className, newItem);
            }
        }

        // Debug output for verification
        foreach (var item in totalInventory.Values)
        {
            //Debug.Log($"Item: {item.className}, Display Name: {item.displayName}, Image Name: {item.imageName}, Stack Size: {item.stackSize}, Rarity: {item.rarity}, Spawn Biome 1: {item.spawnBiome1}, Spawn Biome 2: {item.spawnBiome2}, Sell Price: {item.sellPrice}, Store Price: {item.storePrice}, Goblin Price: {item.goblinPrice}, Is Potion: {item.isPotion}");
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

    public void SaveInventory(string uid)
    {
        // game object find gameplayManager, get component
        if(uid != null)
        {
            Player saveSystem = GameObject.Find("GameplayManager").GetComponent<Player>();
            if(saveSystem.totalInventories.ContainsKey(uid))
            {
                saveSystem.totalInventories[uid] = totalInventory;
                saveSystem.formattedInventories[uid] = formattedInventory;
                saveSystem.SaveGame();
                Debug.Log("Saved inventory for: " + uid);
            }
            else
            {
                saveSystem.totalInventories.Add(uid, totalInventory);
                saveSystem.formattedInventories.Add(uid, formattedInventory);
                saveSystem.SaveGame();
                Debug.Log("Saved inventory for: " + uid);
            }
        }
        

        // do the save system save here
    }

    public void LoadInventory(string uid)
    {
        // game object find gameplayManager, get component
        if(uid != null)
        {
            Player saveSystem = GameObject.Find("GameplayManager").GetComponent<Player>();
            saveSystem.LoadGame();
            if(saveSystem.totalInventories.ContainsKey(uid))
            {
                Debug.Log("Loaded inventory for: " + uid);
                totalInventory = saveSystem.totalInventories[uid];
                formattedInventory = saveSystem.formattedInventories[uid];
            }
            else
            {
                SaveInventory(uid);
            }
        }
        

        // do the save system save here
    }

}
