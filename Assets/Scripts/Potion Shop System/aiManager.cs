using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

public class aiManager : MonoBehaviour
{
    public InventoryLoader inventoryLoader;
    public GameObject npcPrefab; // Reference to the NPC prefab to spawn

    public Transform[] spawnPoints; // Array to hold the spawn points for the NPC

    public GameObject currentNPC; // Reference to the current NPC instance
    public bool playerHasItem = false;

    // Update is called once per frame
    void Update()
    {
        // Check if there are items in stock
        if (inventoryLoader.containerInv.inventory.formattedInventory.Count != 0)
        {
            // If there are items in stock and no NPC instance exists, spawn one
            if (currentNPC == null)
            {
                SpawnNPC();
            }
        }
        else
        {
            // If there are no items in stock and an NPC instance exists, destroy it
            if (currentNPC != null)
            {
                Destroy(currentNPC);
            }
        }
    }

    // Method to spawn an NPC
    private void SpawnNPC()
    {
        // Randomly select a spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Instantiate the NPC prefab at the chosen spawn point
        currentNPC = Instantiate(npcPrefab, spawnPoint.position, Quaternion.identity);
    }
}
