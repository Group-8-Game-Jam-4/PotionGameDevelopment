using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryOpener : MonoBehaviour
{
    public GameObject inventoryObject;
    public bool isPlayer = false;
    public bool isContainer = false;
    public bool isWorkstation = false;
    public GameObject interactText;
    private Transform player;
    private bool playerInRange = false;
    public bool inUi = false;
    InventoryOpener[] inventoryOpeners;
    public bool pressed = false;

    // Update is called once per frame
    void Start()
    {
        // Find all GameObjects with InventoryOpener components
        inventoryOpeners = FindObjectsOfType<InventoryOpener>();
    }

    bool InInventory()
    {
        // Loop through each InventoryOpener found
        foreach (InventoryOpener opener in inventoryOpeners)
        {
            // Check if inUi is true
            if (opener.inUi || inUi)
            {
                // Set canOpen to false
                return true;
            }
        }
        return false;
    }

    void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(!pressed)
            {
                pressed = true;
                if(InInventory())
                {
                    inventoryObject.SetActive(false);
                    inUi = false;
                    return;
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {

            if(InInventory())
            {
                inventoryObject.SetActive(false);
                inUi = false;
                return;
            }
        }


        if(isPlayer)
        {
            if(Input.GetKeyDown(KeyCode.Tab) && !InInventory() && pressed)
            {
                pressed = true;
                inventoryObject.SetActive(true);
                inUi = true;
                inventoryObject.GetComponent<InventoryLoader>().RefreshInventories();
                return;
            }
        }

        if(isContainer && !isWorkstation)
        {
            // Check if the player is in range and presses the "E" key
            if (playerInRange && Input.GetKeyDown(KeyCode.E) && !InInventory())
            {
                // display the inventories
                inventoryObject.SetActive(true);
                inUi = true;
                inventoryObject.GetComponent<InventoryLoader>().RefreshInventories();
                return;
            }
        }
        if(isContainer && isWorkstation)
        {
            // Check if the player is in range and presses the "E" key
            if (playerInRange && Input.GetKeyDown(KeyCode.E) && !InInventory())
            {
                // display the inventories
                inventoryObject.SetActive(true);
                inUi = true;
                inventoryObject.GetComponent<WorkstationInventoryController>().RefreshInventories();
                return;
            }
        }



        if(Input.GetKeyUp(KeyCode.Tab)){pressed = false;}
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!isContainer){return;}
        interactText.SetActive(true);
        // Debug.Log("Player entered trigger");
        player = other.gameObject.transform;
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            // Debug.Log("Player entered trigger zone of NPC.");
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(!isContainer){return;}
        interactText.SetActive(false);
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            // Debug.Log("Player exited trigger zone of NPC.");
            playerInRange = false;
            inventoryObject.SetActive(false);
            inUi = false;
        }
    }
}
