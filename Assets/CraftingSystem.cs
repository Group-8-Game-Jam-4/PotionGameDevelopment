using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    // description
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public GameObject descriptionImage;

    // panels
    public GameObject panelPrefab;
    public GameObject panelContainer;
    private List<GameObject> panelPrefabs = new List<GameObject>();

    // recipe stuff
    private int currentRecipeIndex = 0;
    private Dictionary<string, RecipeClass> recipeClasses = new Dictionary<string, RecipeClass>();
    private List<string> recipeOptions = new List<string>();
    private List<string> craftingOptions = new List<string>();
    private RecipeClass currentRecipe;


    // inventory stuff
    public WorkstationSystem workstationSystem;
    Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        LoadRecipes();
        currentRecipe = recipeClasses[craftingOptions[currentRecipeIndex]];
        inventory = workstationSystem.inputInventoryLoader.playerInv.inventory;
        RecipeUpdate();
    }

    void RecipeUpdate()
    {
        currentRecipe = recipeClasses[craftingOptions[currentRecipeIndex]];

        if(panelPrefabs.Count != 0)
        {
            foreach(GameObject item in panelPrefabs)
            {
                Destroy(item);
            }
            panelPrefabs = new List<GameObject>();
        }

        // set description values
        title.text = "Potion Book - " + currentRecipe.displayName;
        description.text = currentRecipe.description;
        descriptionImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(currentRecipe.imageName);

        // create panels
        if(currentRecipe.itemOne != null)
        {
            if(currentRecipe.itemOne != "")
            {
                // this means theres an item needed for itemOne
                GameObject newPanel = Instantiate(panelPrefab, panelContainer.transform);

                // THIS IS THE NEEDED ITEM

                // gets the input info for item one and 2
                string itemOneClassName;
                string itemTwoClassName;

                // if the item we need is craftable and not collectable
                if(recipeClasses.ContainsKey(currentRecipe.itemOne))
                {
                    newPanel.GetComponent<CraftingPanel>().CraftingParent.SetActive(true);
                    newPanel.GetComponent<CraftingPanel>().HarvestText.text = "";

                    itemOneClassName = recipeClasses[currentRecipe.itemOne].itemOne;
                    itemTwoClassName = recipeClasses[currentRecipe.itemOne].itemTwo;

                    // sets the input info for item one
                    newPanel.GetComponent<CraftingPanel>().input1Text.text = inventory.GetDisplayName(itemOneClassName);
                    newPanel.GetComponent<CraftingPanel>().input1.GetComponent<Image>().sprite = Resources.Load<Sprite>(itemOneClassName);

                    // sets the input info for item two
                    newPanel.GetComponent<CraftingPanel>().input2Text.text = inventory.GetDisplayName(itemTwoClassName);
                    newPanel.GetComponent<CraftingPanel>().input2.GetComponent<Image>().sprite = Resources.Load<Sprite>(itemTwoClassName);

                    // sets the workstation image and text
                    string neededWorkstation = recipeClasses[currentRecipe.itemOne].workstation + "_icon";
                    newPanel.GetComponent<CraftingPanel>().workstation.GetComponent<Image>().sprite = Resources.Load<Sprite>(neededWorkstation);
                    newPanel.GetComponent<CraftingPanel>().workstationText.text = recipeClasses[currentRecipe.itemOne].workstation;

                    // sets the output item info
                    newPanel.GetComponent<CraftingPanel>().outputText.text = inventory.GetDisplayName(recipeClasses[currentRecipe.itemOne].className);
                    newPanel.GetComponent<CraftingPanel>().output.GetComponent<Image>().sprite = Resources.Load<Sprite>(recipeClasses[currentRecipe.itemOne].className);
                }
                else
                {
                    newPanel.GetComponent<CraftingPanel>().CraftingParent.SetActive(false);
                    newPanel.GetComponent<CraftingPanel>().HarvestText.text = "Item must be harvested/found";

                    // sets the output item info
                    newPanel.GetComponent<CraftingPanel>().outputText.text = inventory.GetDisplayName(currentRecipe.itemOne);
                    newPanel.GetComponent<CraftingPanel>().output.GetComponent<Image>().sprite = Resources.Load<Sprite>(currentRecipe.itemOne);
                }

                // adds it to the list
                panelPrefabs.Add(newPanel);
            }
        }

        if(currentRecipe.itemTwo != null)
        {
            if(currentRecipe.itemTwo != "")
            {
                // this means theres an item needed for itemOne
                GameObject newPanel = Instantiate(panelPrefab, panelContainer.transform);

                // THIS IS THE NEEDED ITEM

                // gets the input info for item one and 2
                string itemOneClassName;
                string itemTwoClassName;

                // if the item we need is craftable and not collectable
                if(recipeClasses.ContainsKey(currentRecipe.itemTwo))
                {
                    newPanel.GetComponent<CraftingPanel>().CraftingParent.SetActive(true);
                    newPanel.GetComponent<CraftingPanel>().HarvestText.text = "";

                    itemOneClassName = recipeClasses[currentRecipe.itemTwo].itemOne;
                    itemTwoClassName = recipeClasses[currentRecipe.itemTwo].itemTwo;

                    // sets the input info for item one
                    newPanel.GetComponent<CraftingPanel>().input1Text.text = inventory.GetDisplayName(itemOneClassName);
                    newPanel.GetComponent<CraftingPanel>().input1.GetComponent<Image>().sprite = Resources.Load<Sprite>(itemOneClassName);

                    // sets the input info for item two
                    newPanel.GetComponent<CraftingPanel>().input2Text.text = inventory.GetDisplayName(itemTwoClassName);
                    newPanel.GetComponent<CraftingPanel>().input2.GetComponent<Image>().sprite = Resources.Load<Sprite>(itemTwoClassName);

                    // sets the workstation image and text
                    string neededWorkstation = recipeClasses[currentRecipe.itemTwo].workstation + "_icon";
                    newPanel.GetComponent<CraftingPanel>().workstation.GetComponent<Image>().sprite = Resources.Load<Sprite>(neededWorkstation);
                    newPanel.GetComponent<CraftingPanel>().workstationText.text = recipeClasses[currentRecipe.itemTwo].workstation;

                    // sets the output item info
                    newPanel.GetComponent<CraftingPanel>().outputText.text = inventory.GetDisplayName(recipeClasses[currentRecipe.itemTwo].className);
                    newPanel.GetComponent<CraftingPanel>().output.GetComponent<Image>().sprite = Resources.Load<Sprite>(recipeClasses[currentRecipe.itemTwo].className);
                }
                else
                {
                    newPanel.GetComponent<CraftingPanel>().CraftingParent.SetActive(false);
                    newPanel.GetComponent<CraftingPanel>().HarvestText.text = "Item must be harvested/found";

                    // sets the output item info
                    newPanel.GetComponent<CraftingPanel>().outputText.text = inventory.GetDisplayName(currentRecipe.itemTwo);
                    newPanel.GetComponent<CraftingPanel>().output.GetComponent<Image>().sprite = Resources.Load<Sprite>(currentRecipe.itemTwo);
                }

                // adds it to the list
                panelPrefabs.Add(newPanel);
            }
        }

    }

    public void UpdateItems(string itemOne, string itemTwo)
    {
        panelPrefabs[0].GetComponent<CraftingPanel>().SetStrike(false);
        panelPrefabs[1].GetComponent<CraftingPanel>().SetStrike(false);
        // strikes thru items 1 or 2
        if(itemOne == currentRecipe.itemOne)
        {
            panelPrefabs[0].GetComponent<CraftingPanel>().SetStrike(true);
        }
        if(itemOne == currentRecipe.itemTwo)
        {
            panelPrefabs[1].GetComponent<CraftingPanel>().SetStrike(true);
        }
        if(itemTwo == currentRecipe.itemTwo)
        {
            panelPrefabs[1].GetComponent<CraftingPanel>().SetStrike(true);
        }
    }

    // called by the buttons
    public void PrevRecipe()
    {
        // increment index
        if(currentRecipeIndex == 0)
        {
            currentRecipeIndex = craftingOptions.Count - 1;
        }
        else
        {
            currentRecipeIndex--;
        }
        RecipeUpdate();
    }

    public void NextRecipe()
    {
        // increment index
        if(currentRecipeIndex == craftingOptions.Count - 1)
        {
            currentRecipeIndex = 0;
        }
        else
        {
            currentRecipeIndex++;
        }
        RecipeUpdate();
    }

    public void LoadRecipes()
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
            if (values.Length < 5)
            {
                Debug.LogError($"Incomplete data for line {i + 1}: {lines[i]}");
                continue;
            }
            else
            {

                    // Attempt to parse values
                    RecipeClass newRecipe = new RecipeClass();

                    // Assign other values
                    newRecipe.className = values[0];
                    newRecipe.displayName = values[1];
                    newRecipe.imageName = values[2];
                    newRecipe.workstation = values[3];
                    newRecipe.description = values[6];


                    if(values[4] != null && values[4] != "")
                    {
                        newRecipe.itemOne = values[4];
                    }
                    else
                    {
                        newRecipe.itemOne = "";
                    }

                    if(values[6] != null && values[6] != "")
                    {
                        newRecipe.description = values[6];
                    }
                    else
                    {
                        newRecipe.description = "";
                    }

                    if(values.Length >= 6)
                    {
                        if(values[5] != null && values[5] != "")
                        {
                            newRecipe.itemTwo = values[5];
                        }
                        else
                        {
                            newRecipe.itemTwo = "";
                        }
                    }
                    else
                    {
                        newRecipe.itemTwo = "";
                    }
                    
                    if(newRecipe.itemOne != "")
                    {
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
                    }
                    else
                    {
                        Debug.Log($"Recipe for {values[0]} is a single item recipe");
                        newRecipe.itemOneQuantity = 0;
                    }


                    if(newRecipe.itemTwo != "")
                    {
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
                    }
                    else
                    {
                        Debug.Log($"Recipe for {values[0]} is a single item recipe'");
                        newRecipe.itemTwoQuantity = 0;
                    }


                    // Check if className exists in totalInventory
                    if (recipeClasses.ContainsKey(values[0]))
                    {
                        // Retrieve existing item
                        RecipeClass existingItem = recipeClasses[values[0]];

                        // Check if any property has changed (excluding stackSize)
                        if (
                            existingItem.displayName != newRecipe.displayName ||
                            existingItem.imageName != newRecipe.imageName ||
                            existingItem.workstation != newRecipe.workstation ||
                            existingItem.itemOne != newRecipe.itemOne ||
                            existingItem.itemTwo != newRecipe.itemTwo ||
                            existingItem.itemOneQuantity != newRecipe.itemOneQuantity ||
                            existingItem.itemTwoQuantity != newRecipe.itemTwoQuantity ||
                            existingItem.description != newRecipe.description
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
                            existingItem.description = newRecipe.description;

                            recipeClasses[values[0]] = existingItem;

                            Debug.Log($"Recipe {existingItem.className} updated in recipeOptions.");
                            if(existingItem.workstation == "Crafting Table"){craftingOptions.Add(existingItem.className);}
                            else{recipeOptions.Add(newRecipe.className);}
                        }
                    }
                    else
                    {
                        // Add newPotion to totalInventory
                        recipeClasses.Add(newRecipe.className, newRecipe);
                        if(newRecipe.workstation == "Crafting Table"){craftingOptions.Add(newRecipe.className);}
                        else{recipeOptions.Add(newRecipe.className);}
                    }
            }
        }

        // Debug output for verification
        foreach (var item in recipeClasses.Values)
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
