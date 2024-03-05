using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int inventoryMaxLength = 10;
    private Dictionary<string, ItemClass> totalInventory;
    private List<string[]> formattedInventory;

    public List<string[]> GetInventory()
    {
        LoadInventory();
        return formattedInventory;
    }

    public int AddItem(string itemName, int quantity)
    {
        LoadInventory();

        // if the item exists
        if(totalInventory.ContainsKey(itemName))
        {
            // add it to the formatted inventory
            bool itemsAllAdded = false;
            foreach (string[] array in formattedInventory)
            {
                // if there is items and if we have any of them
                if(array[0] == itemName)
                {
                    // check to see if the stack isnt full
                    int currentQuantity;
                    if (int.TryParse(array[1], out currentQuantity))
                    {
                        if (currentQuantity < totalInventory[array[0]].stackSize)
                        {
                            {
                                // adds what it can to the formatted inventory

                                // howm any items have been added to the stack
                                int addingQuantity = 0;

                                // howm any more items can fit in that stack
                                int stackSpace = 0;
                                if (int.TryParse(array[1], out currentQuantity))
                                {
                                    stackSpace = totalInventory[array[0]].stackSize - currentQuantity;
                                    // Use stackSpace as needed
                                }

                                // if we can fit all the items we want to add into the stack just do it
                                if(quantity <= stackSpace)
                                {
                                    if (int.TryParse(array[1], out currentQuantity))
                                    {
                                        array[1] = (currentQuantity + quantity).ToString();
                                    }

                                    // adds it to the total inventory
                                    totalInventory[itemName].quantity += quantity;

                                    quantity = 0;

                                    itemsAllAdded = true;

                                    Debug.Log($"InventoryStatus: Added {quantity} {itemName}s to the inventory");
                                    break;
                                }
                                else
                                {
                                    // adds as many as we can and then leaves the remainder in quantity
                                    addingQuantity = stackSpace;
                                    quantity -= addingQuantity;
                                    
                                    if (int.TryParse(array[1], out currentQuantity))
                                    {
                                        array[1] = (currentQuantity + addingQuantity).ToString();
                                    }

                                    // adds it to the total inventory
                                    totalInventory[itemName].quantity += addingQuantity;

                                    Debug.Log($"InventoryStatus: Added {addingQuantity} {itemName}s to the inventory with {quantity} remaining");

                                    // we wont break here. This means that once this is added it will keep iterating the inventory to see if theres any more stacks it can add to.
                                    // If theres any space left it will make a new stack if not it will give up
                                }
                            }
                        }
                    }
                }
            }

            if(!itemsAllAdded)
            {
                // if theres space for another stack in the inventory
                if(formattedInventory.Count < inventoryMaxLength)
                {
                    addNewStack(itemName, quantity);
                    Debug.Log($"InventoryStatus: Added {quantity} {itemName}s to the inventory as a new stack");
                }
                else
                {
                    Debug.Log($"InventoryError: No inventory space left for {quantity} {itemName}s");
                }
            }
        }
        else
        {
            Debug.Log($"InventoryError: Invalid item {itemName}");
        }

        SaveInventory();

        // if we cant add all the items needed we will return however many are left, if we can add them all we will return 0 which the script can check to make sure we got the items
        return quantity;
    }

    void addNewStack(string itemName, int quantity)
    {
        // create a new string array with itemName and quantity
        string[] newItem = { itemName, quantity.ToString() };

        // add the new item to formattedInventory
        formattedInventory.Add(newItem);

        // adds it to the total inventory
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

                                Debug.Log($"InventoryStatus: Removed {quantity} {itemName}s from the inventory");
                                break;
                            }
                        }
                        else
                        {
                            // adds as many as we can and then leaves the remainder in quantity
                            if (int.TryParse(array[1], out takingQuantity))
                            {
                                quantity -= takingQuantity;
                            }

                            // we need to totally remove this stack from the inv since its been zeroed
                            formattedInventory.Remove(array);

                            // removes it from the total inventory
                            totalInventory[itemName].quantity -= takingQuantity;

                            Debug.Log($"InventoryStatus: Removed {takingQuantity} {itemName}s from the inventory with {quantity} remaining to take");

                            // we wont break here. This means that once this is added it will keep iterating the inventory to take from the other stacks.
                        }
                    }
                }
            }
            else
            {
                Debug.Log($"InventoryError: Inventory does not contain {quantity} {itemName}s so they cannot be taken");
                return false;
            }
            return true;
        }
        else
        {
            Debug.Log($"InventoryError: Invalid item {itemName}");
        }
        return false;

        // get the inventory and see if there is any of the item to take, if there is -1 of that item and return true. This method will be used primarily by give item
    }

    public void GiveItem()
    {
        // parse in the npc name we want to give the item to (from gameplay manager or somth) then if take item is true take it
    }

    public void SaveInventory()
    {
        // just save it using the save system here
    }

    public void LoadInventory()
    {
        // we need to load the csv to get all the possible items into the total inventory if they arent there already
    }

    void LoadCSV()
    {
        // // loop thru the csv, see if the className exists in totalInventory as a key. If it doesent make a new instance of the itemclass, add in the info from the csv and add it into totalInventory



        // // imports the csv
        // TextAsset textFile = Resources.Load<TextAsset>("questLines");

        // Debug.Log(textFile);

        // // splits into lines
        // string[] splittedLines = textFile.text.Split("\n");

        // string prevName = "";
        // List<string[]> questsList = new List<string[]>();

        // // splits into items
        // for (int i = 2; i < splittedLines.Length; i++)
        // {
        //     //Debug.Log(splittedLines[i]);
        //     string[] splittedValues = ParseCSVLine(splittedLines[i]);

        //     // if it's a new NPC name, add it to the list and create a new quest list
        //     if (splittedValues[0] != "" && splittedValues[0] != prevName)
        //     {
        //         npcNames.Add(splittedValues[0]);

        //         // Create a new quest list for this NPC
        //         questsList = new List<string[]>();

        //         // Add the new quest list to the hashmap by name
        //         questLines.Add(splittedValues[0], questsList);

        //         // set the new prevName
        //         prevName = splittedValues[0];
        //     }

        //     // adds this line to the quest list
        //     questsList.Add(splittedValues);
        // }
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
