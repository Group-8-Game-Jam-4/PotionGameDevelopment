using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkstationInventoryController : MonoBehaviour
{
    // Start is called before the first frame update
    public InventoryLoader inputInv;
    public InventoryLoader outputInv;
    public void RefreshInventories()
    {
        inputInv.RefreshInventories();
        outputInv.RefreshInventories();
    }
}
