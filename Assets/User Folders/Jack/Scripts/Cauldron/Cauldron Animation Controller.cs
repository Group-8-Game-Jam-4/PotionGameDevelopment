using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronAnimationController : MonoBehaviour
{
    public Animator animator;
    public GameObject sprite;
    public GameObject bubbleHandler;
    public GameObject bubble;
    float startDistance;
    float scaleStart;

    private void Start()
    {
        //get the start distance and starting scale so that we can reset the bubbles
        startDistance = transform.position.y;
        scaleStart = transform.localScale.x;
    }

    public void StartAnimation()
    {
        //set the animation bool to true so animation starts
        animator.SetBool("isPop", true);

        //increment the bubbles popped value
        CauldronBubbles bubbleIncrement = bubbleHandler.GetComponent<CauldronBubbles>();
        bubbleIncrement.bubblesPopped += 1;
        Debug.Log("BUBBLE WAS HIT, INCREMENTING, " + bubbleIncrement.bubblesPopped + " IS THE CURRENT BUBBLE SCORE");
    }

    public void StartAnimationAuto()
    {
        //set the animation bool to true so it can start
        animator.SetBool("isPop", true);

        //no incrementation code here because the player needs to manually pop the bubbles and not just let it happen automatically
    }

    public void DeleteBubble()
    {
        //de-activate the bubbles
        sprite.SetActive(false);

        //get the bubble movement
        BubblesMoving bubbleMovement = bubble.GetComponent<BubblesMoving>();
        
        //reset the bubbles locations
        bubbleMovement.transform.localScale = new Vector2(scaleStart, scaleStart);
        bubbleMovement.transform.position = new Vector2(transform.position.x, startDistance);

        //set registered to false so more can spawn
        bubbleMovement.registered = false;
    }
}
