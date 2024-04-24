using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerInventory : MonoBehaviour
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
        inventory.AddItem("carpet", 1);
        inventory.AddItem("lamp", 1);
        inventory.AddItem("painting", 1);
        inventory.AddItem("rack", 1);
        inventory.AddItem("table", 1);
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
