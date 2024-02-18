using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    //Public Variables
    public float baseMovementSpeed = 5f; //Movement Speed
    public float sprintMultiplier = 2f; // Sprint multipleier

    //Private Variables
    private float currentMovementSpeed;

    void Start()
    {
        currentMovementSpeed = baseMovementSpeed;
    }

    void Update()
    {
        // WASD Movement
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f).normalized;
        transform.position += movement * currentMovementSpeed * Time.deltaTime;

        // Shift Sprint
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentMovementSpeed = Mathf.Lerp(currentMovementSpeed, baseMovementSpeed * sprintMultiplier, Time.deltaTime * 10f);
        }
        else
        {
            currentMovementSpeed = Mathf.Lerp(currentMovementSpeed, baseMovementSpeed, Time.deltaTime * 10f);
        }
    }
}
