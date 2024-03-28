using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionDetector : MonoBehaviour
{
    public itemCollect itemCollect; // Reference to the script containing the function

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == itemCollect.player && itemCollect.hasInteracted && itemCollect.isPickedUp)
        {
                // Call the function from the other script
                itemCollect.RemoveItem();
        }
    }
}
