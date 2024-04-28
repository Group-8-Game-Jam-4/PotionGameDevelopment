using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantingOrbs: MonoBehaviour
{
    public int bubblesPopped;
    public GameObject[] orbObject;
    public WorkstationSystem workStationScript;

    // Update is called once per frame
    void Update()
    {
        //if bubbles popped is greater than or equal to 5
        if (bubblesPopped >= 5)
        {
            //run reset bubbles
            for (int i = 0; i < orbObject.Length; i++)
            {
                //get bubbles
                OrbsMoving orb = orbObject[i].GetComponent<OrbsMoving>();

                //set registered as true so they stop spawning
                orb.registered = true;

                //stop the movement of the bubbles
                orb.CancelInvoke();

                //delete the bubbles
                orbObject[i].transform.GetChild(0).gameObject.SetActive(false);

                orb.ResetOrbs();
            }
            //hooking up to crafting goes here
            workStationScript.BrewPotion();

            //reset bubbles popped
            bubblesPopped = 0;
        }
    }
}