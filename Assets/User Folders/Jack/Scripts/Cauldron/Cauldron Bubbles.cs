using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronBubbles : MonoBehaviour
{
    public int bubblesPopped;
    public GameObject[] bubbleObject;

    // Update is called once per frame
    void Update()
    {
        //if bubbles popped is greater than or equal to 5
        if (bubblesPopped >= 5)
        {
            //run reset bubbles
            for (int i = 0; i < bubbleObject.Length; i++)
            {
                //get bubbles
                BubblesMoving bubble = bubbleObject[i].GetComponent<BubblesMoving>();

                //set registered as true so they stop spawning
                bubble.registered = true;

                //stop the movement of the bubbles
                bubble.CancelInvoke();

                //delete the bubbles
                bubbleObject[i].transform.GetChild(0).gameObject.SetActive(false);

                //hooking up to crafting goes here
            }

            //reset bubbles popped
            bubblesPopped = 0;
        }
    }
}