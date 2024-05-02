using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.GUID;

public class PlayerInventory : MonoBehaviour
{
    public int inventoryMaxLength = 10;
    public Inventory inventory = new Inventory();
    public List<string[]> formattedInventory = new List<string[]>();
    public Dictionary<string, ItemClass> totalInventory = new Dictionary<string, ItemClass>();

    private Player saveSystem;
    public GuidReference uid;

    // Start is called before the first frame update
    void Awake()
    {
        inventory.inventoryMaxLength = inventoryMaxLength;
        LoadInventory();
    }

    public void SaveInventory()
    {
        inventory.SaveInventory("player");
    }

    public void LoadInventory()
    {
        inventory.LoadInventory("player");
        formattedInventory = inventory.formattedInventory;
        totalInventory = inventory.totalInventory;
        inventory.LoadCSV();
        // do the save system load here
    }
}
