using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonValue : MonoBehaviour
{
    // Start is called before the first frame update
    public int value;
    public bool isPlayer;

    public void OnPress()
    {
        transform.parent.parent.parent.parent.gameObject.GetComponent<InventoryLoader>().SelectItem(value, isPlayer);
    }
}
