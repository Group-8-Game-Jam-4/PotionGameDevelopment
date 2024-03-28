using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int inventoryMaxLength = 10;
    public Inventory inventory = new Inventory();
    public List<string[]> formattedInventory = new List<string[]>();
    public Dictionary<string, ItemClass> totalInventory = new Dictionary<string, ItemClass>();

    // Start is called before the first frame update
    void Awake()
    {
        inventory.inventoryMaxLength = inventoryMaxLength;
        formattedInventory = inventory.formattedInventory;
        totalInventory = inventory.totalInventory;
        inventory.LoadCSV();
        inventory.AddItem("stick", 12);
        inventory.AddItem("stick", 5);
        inventory.AddItem("stick", 9);
    }

    public void SaveInventory()
    {
        // do the save system save here
    }

    public void LoadInventory()
    {
        inventory.LoadCSV();
        // do the save system load here
    }
}
