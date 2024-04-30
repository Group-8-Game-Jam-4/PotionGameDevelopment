using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.GUID;

public class ContainerInventory : MonoBehaviour
{
    public int inventoryMaxLength = 10;
    public Inventory inventory = new Inventory();
    public List<string[]> formattedInventory = new List<string[]>();
    public Dictionary<string, ItemClass> totalInventory = new Dictionary<string, ItemClass>();
    public GuidReference uid;

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
        inventory.SaveInventory(uid.guidString.ToString());
    }

    public void LoadInventory()
    {
        inventory.LoadInventory(uid.guidString.ToString());
        formattedInventory = inventory.formattedInventory;
        totalInventory = inventory.totalInventory;
        inventory.LoadCSV();
        // do the save system load here
    }
}
