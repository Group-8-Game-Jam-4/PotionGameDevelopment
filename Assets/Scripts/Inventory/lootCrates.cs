using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class lootCrates : MonoBehaviour
{
    // first thing is get inventory for crate
    // public things for crate, respawn time, item quantity, and thats probs it

    public ContainerInventory containerInv;
    public GameObject worldSprite;

    public int maxRespawnTime = 60;
    public int minRespawnTime = 15;
    public int maxItems = 10;
    public int minItems = 2;
    public int rarityMultiplier = 1;
    bool onTimer = false;

    private List<string> itemTable= new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        containerInv.LoadInventory();
        if(containerInv.formattedInventory.Count != 0)
        {
            worldSprite.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckInventory();
    }

    void CheckInventory()
    {
        if(containerInv.formattedInventory.Count <= 0 && !onTimer)
        {
            onTimer = true;
            StartRespawnTimer();
        }
    }
    // on interact do xyz

    // this will be called from inventory loader but i cant do this until morgans code is done.... or can i?
    void StartRespawnTimer()
    {
        worldSprite.SetActive(false);
        int time = Random.Range(minRespawnTime, maxRespawnTime);
        Invoke("RegenCrate", (float)time);
    }
    
    void RegenCrate()
    {
        // generate rarity table
        foreach(ItemClass item in containerInv.inventory.totalInventory.Values)
        {
            int amount = 1;
            // common, uncommon, rare, epic, legendary
            if(item.rarity == "Common"){amount = 10 * rarityMultiplier;}
            if(item.rarity == "Uncommon"){amount = 8 * rarityMultiplier;}
            if(item.rarity == "Rare"){amount = 3;}
            if(item.rarity == "Epic"){amount = 2;}
            if(item.rarity == "Legendary"){amount = 1;}

            for(int i = 0; i < amount; i++)
            {
                itemTable.Add(item.className);
            }
        }

        // pick how many items
        int itemAmount = Random.Range(minItems, maxItems);

        // add however many
        for(int i = 0; i < itemAmount; i++)
        {
            // pick the random item each time
            int itemNumber = Random.Range(0, itemTable.Count);
            containerInv.inventory.AddItem(itemTable[itemNumber], 1);
        }

        // actually show the crate
        worldSprite.SetActive(true);
        onTimer = false;
    }
}
