using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionDetector : MonoBehaviour
{
    public itemCollect itemCollect;
    public bool readyToCollect = false;

    private void Update()
    {
        if (readyToCollect == true && itemCollect.hasInteracted && itemCollect.isPickedUp)
        {
            itemCollect.RemoveItem();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == itemCollect.player)
        {
            readyToCollect = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        readyToCollect= false;
    }
}
