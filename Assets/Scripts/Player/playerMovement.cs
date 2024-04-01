using Cinemachine;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    // Public Variables
    public float baseMovementSpeed = 5f; // Movement Speed
    public float sprintMultiplier = 2f; // Sprint multiplier
    private Rigidbody2D rb;
    private Animator anim;
    public CinemachineVirtualCamera mainCamera;
    public Transform npc;
    public bool isMoving;
    public int horizontal = 0;
    public int vertical = 0;
    public bool isSwinging = false;
    // Private Variables
    private float currentMovementSpeed;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentMovementSpeed = baseMovementSpeed;


    }
    void swingStop()
    {
        Debug.Log("Setting false swing animation");
        anim.SetBool("swingFront", false);
        anim.SetBool("swingBack", false);
        anim.SetBool("swingLeft", false);
        anim.SetBool("swingRight", false);
        isSwinging = false;
    }

    void Update()
    {
        // WASD Movement
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(horizontalInput, verticalInput).normalized * currentMovementSpeed;

        if (horizontalInput != 0 && !isSwinging)
        {
            movement = new Vector2(horizontalInput, 0).normalized * currentMovementSpeed;
            isMoving = true;
        }
        if (verticalInput != 0 && !isSwinging)
        {
            movement = new Vector2(0, verticalInput).normalized * currentMovementSpeed;
            isMoving = true;
        }
        if (verticalInput == 0 && horizontalInput == 0)
        {
            isMoving = false;
        }
        if(isSwinging)
        {
            isMoving = false;
        }

        rb.velocity = movement;

        // Shift Sprint
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentMovementSpeed = Mathf.Lerp(currentMovementSpeed, baseMovementSpeed * sprintMultiplier, Time.deltaTime * 10f);
        }
        else
        {
            currentMovementSpeed = Mathf.Lerp(currentMovementSpeed, baseMovementSpeed, Time.deltaTime * 10f);
        }

        if (verticalInput > 0)
        {
            vertical = 1;
            horizontal = 0;
        }
        if (verticalInput < 0)
        {
            vertical = -1;
            horizontal = 0;
        }
        if (horizontalInput > 0)
        {
            horizontal = 1;
            vertical = 0;
        }
        if (horizontalInput < 0)
        {
            horizontal = -1;
            vertical = 0;
        }

        // Animations
        anim.SetFloat("Horizontal", movement.x);
        anim.SetFloat("Vertical", movement.y);
        anim.SetFloat("Speed", movement.sqrMagnitude);

        if (horizontalInput != 0 || verticalInput != 0)
        {
            anim.SetBool("Moving", true);
        }


        


        if (!isMoving && horizontal == -1)
        {
            anim.SetBool("walkUp", false);
            anim.SetBool("walkDown", false);
            anim.SetBool("walkLeft", true);
            anim.SetBool("walkRight", false);
            anim.SetBool("Moving", false);
        }
        if (!isMoving && horizontal == 1)
        {
            anim.SetBool("walkUp", false);
            anim.SetBool("walkDown", false);
            anim.SetBool("walkLeft", false);
            anim.SetBool("walkRight", true);
            anim.SetBool("Moving", false);
        }
        if (!isMoving && vertical == 1)
        {
            anim.SetBool("walkUp", true);
            anim.SetBool("walkDown", false);
            anim.SetBool("walkLeft", false);
            anim.SetBool("walkRight", false);
            anim.SetBool("Moving", false);
        }
        if (!isMoving && vertical == -1)
        {
            anim.SetBool("walkUp", false);
            anim.SetBool("walkDown", true);
            anim.SetBool("walkLeft", false);
            anim.SetBool("walkRight", false);
            anim.SetBool("Moving", false);
        }

        if(Input.GetMouseButtonDown(0) && vertical == -1)
        {
            anim.SetBool("swingFront", true);
            isSwinging= true;
        }

        if (Input.GetMouseButtonDown(0) && vertical == 1)
        {
            anim.SetBool("swingBack", true);
            isSwinging = true;
        }
        if (Input.GetMouseButtonDown(0) && horizontal == 1)
        {
            anim.SetBool("swingRight", true);
            isSwinging = true;
        }
        if (Input.GetMouseButtonDown(0) && horizontal == -1)
        {
            anim.SetBool("swingLeft", true);
            isSwinging = true;
        }


    

    }
}
