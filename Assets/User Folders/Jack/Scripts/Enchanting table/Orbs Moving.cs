using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbsMoving : MonoBehaviour
{
    public WorkstationSystem workStationScript;
    public GameObject sprite;
    public GameObject orb;
    float stopDistanceY;
    float stopDistanceX;
    float startDistance;
    float moveSpeed;
    float scaleStart;
    float scaleEnd;
    public bool registered = false;
    // Start is called before the first frame update
    void Start()
    {
        //get the start distance and starting scale so that we can reset the bubbles
        startDistance = transform.position.y;
        scaleStart = transform.localScale.x;
    }

    private void Update()
    {
        //if we're ready to brew shit
        if(workStationScript.readyToBrew == true && registered == false)
        {
            //registered = true so it doesnt loop
            registered = true;

            //randomize the distance the bubbles will travel, their scale and their move speed
            //stopDistance = Random.Range(300, 350);

            //stopDistance = Screen.height/1.9;
            stopDistanceY = Random.Range((float)(Screen.height / 1.9), (float)(Screen.height / 4));
            stopDistanceX = Random.Range((float)(Screen.width / 1.1), (float)(Screen.width / 1.6));
            moveSpeed = Random.Range(1, 1.5f);
            scaleEnd = Random.Range(0.25f, 0.35f);

            //set all bubbles to active
            //this might not actually be needed (the for each loop) cuz this script is on all the bubbles anyway
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }

            //this might be highly fucking inefficient but im honestly not sure
            // if this like chokes the players fps then ill try think of an alterative
            //
            //repeat the move bubbles function
            InvokeRepeating("MoveOrbs", 0f, 0.01f);
        }
    }

    void MoveOrbs()
    {
        //if we're not at the stopping distance
        if (transform.position.y > stopDistanceY)
        {
            //move the bubble
            transform.position = new Vector2(transform.position.x, transform.position.y - moveSpeed - Time.deltaTime);
        }
        else if (transform.position.y < stopDistanceY) 
        {
            //move the bubble
            transform.position = new Vector2(transform.position.x, transform.position.y + moveSpeed + Time.deltaTime);
        }

        if (transform.position.x > stopDistanceX)
        {
            //move the bubble
            transform.position = new Vector2(transform.position.x - moveSpeed - Time.deltaTime, transform.position.y);
        }
        else if (transform.position.x < stopDistanceX)
        {
            //move the bubble
            transform.position = new Vector2(transform.position.x + moveSpeed + Time.deltaTime, transform.position.y);
        }

        //if we are at the stopping distance
        if (transform.position.y == stopDistanceY && transform.position.x == stopDistanceX)
        {
            Debug.Log("wunguh");
            //cancel this function so we dont like explode 
            CancelInvoke();
        }

    }

    public void ResetOrbs()
    {
        //de-activate the bubbles
        sprite.SetActive(false);

        //reset the bubbles locations
        orb.transform.localScale = new Vector2(scaleStart, scaleStart);
        orb.transform.position = new Vector2(transform.position.x, startDistance);

        //set registered to false so more can spawn
        registered = false;
    }
}
