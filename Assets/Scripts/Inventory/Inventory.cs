using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public int inventoryMaxLength = 10;
    public GameObject UITemplate;
    public GameObject inventory;
    private Dictionary<string, ItemClass> totalInventory = new Dictionary<string, ItemClass>();
    private List<string[]> formattedInventory = new List<string[]>();

    private void Start() 
    {
        LoadCSV();
        AddItem("stick", 12);
        AddItem("stick", 5);
        AddItem("stick", 9);
        PopulateInventoryUI();
    }

    private void PopulateInventoryUI()
    {
        // foreach thing in formatted inventory get name and quantity (values 1 and 2) Each one of these will be an object in the ui. Each object will also need an image and the display name which we will get from the item class by querying the total inventory

        GameObject playerItemList;
        GameObject containerItemList;

        // Find the InventoryUI object in the scene
        GameObject inventoryUI = GameObject.Find("InventoryUI");

        // Find the child object named ItemsListContent
        GameObject inventories = inventoryUI.transform.Find("Inventories").gameObject;
        playerItemList = inventories.transform.Find("Player").transform.Find("ItemsListContent").gameObject;
        containerItemList = inventories.transform.Find("Container").transform.Find("ItemsListContent").gameObject;


        // first we populate the player inventory

        foreach (string[] array in formattedInventory)
        {
            // Prep the object
            GameObject UIElement = Instantiate(UITemplate, playerItemList.transform);

            // Get child text object named title (as TMPro Text)
            TextMeshProUGUI titleText = UIElement.transform.Find("Title").GetComponent<TextMeshProUGUI>();

            // Get child image named Image (as a UI Image)
            Image imageComponent = UIElement.transform.Find("Image").GetComponent<Image>();

            // Get child text (child of the image object) as TMPro Text (named quantity)
            TextMeshProUGUI quantityText = imageComponent.transform.Find("Quantity").GetComponent<TextMeshProUGUI>();

            // get the info for it

            quantityText.text = array[1].ToString();
            titleText.text = array[0];
        }

        // add the empty ones

        int a = inventoryMaxLength - formattedInventory.Count();

        if(a > 0)
        {
            for(int i = 0; i < a; i++)
            {
                // Prep the object
                GameObject UIElement = Instantiate(UITemplate, playerItemList.transform);

                // Get child text object named title (as TMPro Text)
                TextMeshProUGUI titleText = UIElement.transform.Find("Title").GetComponent<TextMeshProUGUI>();

                // Get child image named Image (as a UI Image)
                Image imageComponent = UIElement.transform.Find("Image").GetComponent<Image>();

                // Get child text (child of the image object) as TMPro Text (named quantity)
                TextMeshProUGUI quantityText = imageComponent.transform.Find("Quantity").GetComponent<TextMeshProUGUI>();

                // get the info for it

                quantityText.text = "";
                titleText.text = "";
            }
        }
    }

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
                    Debug.LogError($"InventoryError: No inventory space left for {quantity} {itemName}s");
                }
            }
        }
        else
        {
            Debug.LogError($"InventoryError: Invalid item {itemName}");
        }

        SaveInventory();

        // if we cant add all the items needed we will return however many are left, if we can add them all we will return 0 which the script can check to make sure we got the items
        return quantity;
    }


    // this needs to be modified to confirm its adding the correct quantity that will fit in a stack (or potentially change it above line 150. This is because when you add idk like 40 items if you had none of them to begin with it just adds them as 1 stack)
    // this could be an issue but it really shouldnt be because you can only ever move or recieve a stack of items at a time
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

    private void LoadCSV()
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
        string[] lines = textFile.text.Split('\n');

        // Parse each line of the CSV
        for (int i = 2; i < lines.Length; i++)
        {
            string[] values = ParseCSVLine(lines[i]);

            // Check if all required values are present
            if (values.Length < 10)
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
                    existingItem.goblinPrice != newItem.goblinPrice)
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
            Debug.Log($"Item: {item.className}, Display Name: {item.displayName}, Image Name: {item.imageName}, Stack Size: {item.stackSize}, Rarity: {item.rarity}, Spawn Biome 1: {item.spawnBiome1}, Spawn Biome 2: {item.spawnBiome2}, Sell Price: {item.sellPrice}, Store Price: {item.storePrice}, Goblin Price: {item.goblinPrice}");
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
