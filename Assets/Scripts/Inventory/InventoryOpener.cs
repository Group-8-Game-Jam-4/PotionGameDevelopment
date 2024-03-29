using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryOpener : MonoBehaviour
{
    public GameObject inventoryObject;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(inventoryObject.active)
            {
                inventoryObject.SetActive(false);
            }
            else
            {
                inventoryObject.SetActive(true);
                inventoryObject.GetComponent<InventoryLoader>().RefreshInventories();
            }
        }
    }
}
