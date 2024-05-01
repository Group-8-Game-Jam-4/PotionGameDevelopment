using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateInventory : MonoBehaviour
{
    public int inventoryMaxLength = 10;
    public Inventory inventory = new Inventory();
    public List<string[]> formattedInventory = new List<string[]>();
    public Dictionary<string, ItemClass> totalInventory = new Dictionary<string, ItemClass>();

    // this is purely to make it easier in editor
    public string InventoryName;

    // Start is called before the first frame update
    void Awake()
    {
        inventory.inventoryMaxLength = inventoryMaxLength;
        LoadInventory();
    }

    public void SaveInventory()
    {
        
    }

    public void LoadInventory()
    {
        inventory.LoadCSV();
        formattedInventory = inventory.formattedInventory;
        totalInventory = inventory.totalInventory;
    }
}
