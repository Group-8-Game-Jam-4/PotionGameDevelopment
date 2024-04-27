using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcController : MonoBehaviour
{
    public Transform[] shopCounters;
    public Transform[] exitWaypoints;
    public GameObject exclamationMarkPrefab; // Prefab for the exclamation mark sprite
    private int currentShopCounterIndex = -1;
    private int currentExitWaypointIndex = -1;
    public float movementSpeed = 5f;
    public InventoryLoader inventoryLoader;
    public List<string> potionOptions = new List<string>();

    void Start()
    {
        inventoryLoader = GameObject.Find("ShopStock").transform.Find("PotionStockUI").GetComponent<InventoryLoader>();
        shopCounters = new Transform[3];
        shopCounters[0] = GameObject.Find("FrontDesk").transform;
        shopCounters[1] = GameObject.Find("FrontDesk1").transform;
        shopCounters[2] = GameObject.Find("FrontDesk2").transform;

        exitWaypoints = new Transform[2];
        exitWaypoints[0] = GameObject.Find("ExitShop").transform;
        exitWaypoints[1] = GameObject.Find("ExitShop2").transform;

        currentShopCounterIndex = Random.Range(0, shopCounters.Length);

        foreach (var item in inventoryLoader.containerInv.formattedInventory)
        {
            potionOptions.Add(item[0]);
        }

        ChangePotionSpriteRandomly();

        if (exclamationMarkPrefab == null)
        {
            Debug.LogError("Exclamation mark prefab is not assigned!");
        }

    }

    void Update()
    {
        if (currentShopCounterIndex >= 0)
        {
            // Check if NPC is close enough to the shop counter
            if (Vector3.Distance(transform.position, shopCounters[currentShopCounterIndex].position) < 0.1f)
            {
                // Activate exclamation mark
                ActivateExclamationMark();

                // Move to the exit waypoint
                MoveToNextExitWaypoint();
                currentShopCounterIndex = -1;
            }
            else
            {
                // Move towards the shop counter
                MoveToNextShopCounter();
            }
        }
        else if (currentExitWaypointIndex >= 0 && Vector3.Distance(transform.position, exitWaypoints[currentExitWaypointIndex].position) < 0.1f)
        {
            Destroy(gameObject);
        }
    }


    void MoveToNextShopCounter()
    {
        Debug.Log("Moving");
        Vector3 direction = (shopCounters[currentShopCounterIndex].position - transform.position).normalized;
        transform.position += direction * movementSpeed * Time.deltaTime;
    }

    void MoveToNextExitWaypoint()
    {
        currentExitWaypointIndex = Random.Range(0, exitWaypoints.Length);

        Vector3 direction = (exitWaypoints[currentExitWaypointIndex].position - transform.position).normalized;
        transform.position += direction * movementSpeed * Time.deltaTime;
    }

    void ChangePotionSpriteRandomly()
    {
        Transform potionImage = transform.Find("potionimage");
        if (potionImage != null)
        {
            int randomIndex = Random.Range(0, potionOptions.Count);
            string potionName = potionOptions[randomIndex];

            Sprite potionSprite = Resources.Load<Sprite>(potionName);

            if (potionSprite != null)
            {
                SpriteRenderer spriteRenderer = potionImage.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = potionSprite;
            }
            else
            {
                Debug.LogError("Sprite for potion '" + potionName + "' not found in the Resources folder.");
            }
        }
        else
        {
            Debug.LogError("Child object 'potionimage' not found.");
        }
    }

    void ActivateExclamationMark()
    {
        // Instantiate exclamation mark above a random shelf
        int randomShelfIndex = Random.Range(0, 8); // Generate a random shelf index (0 to 7)
        string shelfName = "Shelves_0 (" + randomShelfIndex + ")"; // Construct the shelf name
        Transform shelf = GameObject.Find(shelfName).transform; // Find the shelf by name

        if (shelf != null && exclamationMarkPrefab != null)
        {
            Instantiate(exclamationMarkPrefab, shelf.position + Vector3.up * 2f, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Shelf or exclamation mark prefab not found!");
        }
    }

}