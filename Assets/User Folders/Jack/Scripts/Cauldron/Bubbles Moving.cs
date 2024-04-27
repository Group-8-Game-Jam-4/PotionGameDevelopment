using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BubblesMoving : MonoBehaviour
{
    public GameObject bubble;
    public GameObject bubbleSprite;
    double stopDistance;
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
        //temp just for testing, gonna be triggered by crafting later
        if(registered == false)
        {
            //registered = true so it doesnt loop
            registered = true;

            //randomize the distance the bubbles will travel, their scale and their move speed
            //stopDistance = Random.Range(300, 350);

            //stopDistance = Screen.height/1.9;
            stopDistance = Random.Range((float)(Screen.height / 1.9), (float)(Screen.height / 2.05));
            moveSpeed = Random.Range(0.2f, 0.4f);
            scaleEnd = Random.Range(0.25f, 0.35f);

            //set all bubbles to active
            //this might not actually be needed (the for each loop) cuz this script is on all the bubbles anyway
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }

            //move all bubbles
            bubble.gameObject.transform.localScale = new Vector2(scaleEnd, scaleEnd);

            //this might be highly fucking inefficient but im honestly not sure
            // if this like chokes the players fps then ill try think of an alterative
            //
            //repeat the move bubbles function
            InvokeRepeating("MoveBubbles", 0f, 0.005f);
        }
    }

    void MoveBubbles()
    {
        //if we're not at the stopping distance
        if (transform.position.y < stopDistance)
        {
            //move the bubble
            transform.position = new Vector2(transform.position.x, transform.position.y + moveSpeed + Time.deltaTime);
        }

        //if we are at the stopping distance
        if (transform.position.y >= stopDistance)
        {
            //cancel this function so we dont like explode someones computer
            CauldronAnimationController bubbleAnimation = bubbleSprite.GetComponent<CauldronAnimationController>();
            bubbleAnimation.StartAnimationAuto();
            CancelInvoke();
        }
    }
}
