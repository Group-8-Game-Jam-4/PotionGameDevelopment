using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class InventoryLoader : MonoBehaviour
{
    // inventory
    public bool playerInventory = true;
    public bool containerInventory = false;
    public GameObject UITemplate;
    public GameObject sliderElement;
    public TextMeshProUGUI sliderQuantityText;
    public PlayerInventory playerInv;
    public ContainerInventory containerInv;
    ItemClass selectedItem;
    bool selectedPlayerItem;

    private void Start() 
    {
        RefreshInventories();
    }

    public void OnSliderChange(float value)
    {
        sliderQuantityText.text = value.ToString();
    }

    public void SelectItem(int value, bool isPlayer)
    {
        if(isPlayer)
        {
            //get the item
            string[] item = playerInv.formattedInventory[value];

            // if the item like exists. It should be physically impossible for it to not but yknow
            if(playerInv.totalInventory.ContainsKey(item[0]))
            {
                selectedItem = playerInv.totalInventory[item[0]];
                selectedPlayerItem = true;

                // set slider max value to amount of that item
                if (int.TryParse(item[1], out int currentQuantity))
                {
                    transform.Find("Give Items Slider").transform.Find("Slider").gameObject.GetComponent<Slider>().maxValue = currentQuantity;
                }
            }
        }
        else
        {
            //get the item
            string[] item = containerInv.formattedInventory[value];

            // if the item like exists. It should be physically impossible for it to not but yknow
            if(containerInv.totalInventory.ContainsKey(item[0]))
            {
                selectedItem = containerInv.totalInventory[item[0]];
                selectedPlayerItem = false;

                // set slider max value to amount of that item
                if (int.TryParse(item[1], out int currentQuantity))
                {
                    transform.Find("Give Items Slider").transform.Find("Slider").gameObject.GetComponent<Slider>().maxValue = currentQuantity;
                }
            }
        }
        sliderElement.SetActive(true);
    }

    public void TransferItems()
    {
        // get how many items to transfer
        int sliderValue = (int)transform.Find("Give Items Slider").transform.Find("Slider").gameObject.GetComponent<Slider>().value;

        // if we are moving an item from the player inventory
        if(selectedPlayerItem)
        {
            // if we can actually take that many
            if(playerInv.inventory.TakeItem(selectedItem.className, sliderValue))
            {
                if(containerInventory == true)
                {
                    // if we can actually fit that many
                    if(containerInv.inventory.AddItem(selectedItem.className, sliderValue));
                }
            }
        }
        else
        {
            // if we can actually take that many
            if(containerInv.inventory.TakeItem(selectedItem.className, sliderValue))
            {
                // if we can actually fit that many
                if(playerInv.inventory.AddItem(selectedItem.className, sliderValue));
            }
        }

        // save the inventories and reload the uis
        playerInv.SaveInventory();
        containerInv.SaveInventory();

        // refresh inventories
        RefreshInventories();
        sliderElement.SetActive(false);
    }

    private void RefreshInventories()
    {
        // if player inventory is enabled show it in the left bit. If not just show the container
        if(playerInventory)
        {
            PopulateInventoryUI(playerInv.formattedInventory, playerInv.inventoryMaxLength, true);
        }
        if(containerInventory)
        {
            // change these to be well not the playerInv. Like the cart inv or something
            PopulateInventoryUI(containerInv.formattedInventory, containerInv.inventoryMaxLength, false);
        }
    }

    // we need to get the stuff from the player inventory to populate that part of the ui if its needed

    private void PopulateInventoryUI(List<string[]> formattedInventory, int inventoryMaxLength, bool isPlayer)
    {
        // foreach thing in formatted inventory get name and quantity (values 1 and 2) Each one of these will be an object in the ui. Each object will also need an image and the display name which we will get from the item class by querying the total inventory

        GameObject ItemList;

        // Find the InventoryUI object in the scene
        GameObject inventoryUI = gameObject;

        // Find the child object named ItemsListContent
        GameObject inventories = inventoryUI.transform.Find("Inventories").gameObject;


        if(isPlayer)
        {
            ItemList = inventories.transform.Find("Player").transform.Find("ItemsListContent").gameObject;
            foreach (Transform child in ItemList.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        else
        {
            ItemList = inventories.transform.Find("Container").transform.Find("ItemsListContent").gameObject;
            foreach (Transform child in ItemList.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }


        // first we populate the player inventory

        for (int i = 0; i < formattedInventory.Count; i++)
        {
            string[] array = formattedInventory[i];

            // Prep the object
            GameObject UIElement = Instantiate(UITemplate, ItemList.transform);

            // Get child text object named title (as TMPro Text)
            TextMeshProUGUI titleText = UIElement.transform.Find("Title").GetComponent<TextMeshProUGUI>();

            // Get child image named Image (as a UI Image)
            Image imageComponent = UIElement.transform.Find("Image").GetComponent<Image>();

            // Get child text (child of the image object) as TMPro Text (named quantity)
            TextMeshProUGUI quantityText = imageComponent.transform.Find("Quantity").GetComponent<TextMeshProUGUI>();

            // get the info for it
            quantityText.text = array[1].ToString();
            titleText.text = array[0];

            UIElement.GetComponent<ButtonValue>().value = i;
            UIElement.GetComponent<ButtonValue>().isPlayer = isPlayer;
        }

        // add the empty ones

        int a = inventoryMaxLength - formattedInventory.Count();

        if(a > 0)
        {
            for(int i = 0; i < a; i++)
            {
                // Prep the object
                GameObject UIElement = Instantiate(UITemplate, ItemList.transform);

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
}
