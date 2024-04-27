using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class shelfInteract : MonoBehaviour
{
    public PlayerInventory playerInventory;
    public InventoryLoader inventoryLoader;
    public npcController npcController;
    public bool entered = false;

    public void Start()
    {
        playerInventory = GameObject.Find("Player").GetComponent<PlayerInventory>();
        inventoryLoader = GameObject.Find("ShopStock").transform.Find("PotionStockUI").GetComponent<InventoryLoader>();
        npcController = GameObject.Find("ShopNPC").GetComponent<npcController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Press 'E' to change scene");
            entered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        entered = false;
    }

    void Update()
    {
        if (entered == true && (Input.GetKeyDown(KeyCode.E)))
        {
            Destroy(gameObject);
            playerInventory.inventory.AddItem(npcController.potion, 1);
            inventoryLoader.containerInv.inventory.TakeItem()
        }
    }
}
