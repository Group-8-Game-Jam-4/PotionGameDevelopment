using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblesMoving : MonoBehaviour
{
    public GameObject bubble;
    int stopDistance;
    float moveSpeed;
    bool registered = false;
    // Start is called before the first frame update
    void Start()
    {
        stopDistance = Random.Range(281, 366);
        moveSpeed = Random.Range(0.3f, 0.6f);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && (registered == false))
        {
            registered = true;
            invokeMovement();
        }
    }

    void invokeMovement()
    {
        InvokeRepeating("moveBubbles", 0f, 0.005f);
    }

    void moveBubbles()
    {
        if (transform.position.y < stopDistance)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + moveSpeed + Time.deltaTime);
        }

        if (transform.position.y >= stopDistance) 
        {
            CancelInvoke();
        }
    }
}
