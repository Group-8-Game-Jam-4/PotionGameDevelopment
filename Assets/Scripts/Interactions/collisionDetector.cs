using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionDetector : MonoBehaviour
{
    public itemCollect itemCollect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == itemCollect.player && itemCollect.hasInteracted && itemCollect.isPickedUp)
        {
                itemCollect.RemoveItem();
        }
    }
}
