using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shelfInteract : MonoBehaviour
{
    private PlayerInventory playerInventory;
    private InventoryLoader inventoryLoader;
    private aiManager aiManager;
    private bool entered = false;

    public void Start()
    {
        playerInventory = GameObject.Find("Player").GetComponent<PlayerInventory>();
        inventoryLoader = GameObject.Find("ShopStock").transform.Find("PotionStockUI").GetComponent<InventoryLoader>();
        aiManager = GameObject.Find("AIManager").GetComponent<aiManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
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
            aiManager.playerHasItem = true;
            Destroy(gameObject);
        }
    }
}
