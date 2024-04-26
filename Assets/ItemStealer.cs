using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStealer : MonoBehaviour
{
    // so were going to need an inventory for this, probably a total inventory as that will just sort of work
    // Start is called before the first frame update
    public ContainerInventory cart;
    public int minStolenItems;
    public int maxStolenItems;
    public ContainerInventory goblinShop;
    
    public void Steal()
    {
        // work out how many items to steal
        int itemsToSteal = Random.Range(minStolenItems, maxStolenItems);

        if(cart.formattedInventory.Count > itemsToSteal)
        {
            for(int i = 0; i < itemsToSteal; i++)
            {
                // get random one from the inv
                int itemToSteal = Random.Range(0, cart.formattedInventory.Count);
                string[] item = cart.formattedInventory[itemsToSteal];
                
                // take the item and put it in our one
                cart.inventory.TakeItem(item[0], 1);
                goblinShop.inventory.AddItem(item[0], 1);
            }
        }
    }
}
