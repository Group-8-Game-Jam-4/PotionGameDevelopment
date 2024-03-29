using Cinemachine;
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
    // Private Variables
    private float currentMovementSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentMovementSpeed = baseMovementSpeed;
    }

    void Update()
    {
        // WASD Movement
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(horizontalInput, verticalInput).normalized * currentMovementSpeed;
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

        // Animations
        anim.SetFloat("Horizontal", movement.x);
        anim.SetFloat("Vertical", movement.y);
        anim.SetFloat("Speed", movement.sqrMagnitude);

    }
}
