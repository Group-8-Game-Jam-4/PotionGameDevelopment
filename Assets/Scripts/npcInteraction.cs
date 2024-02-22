using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;

public class npcInteraction : MonoBehaviour
{
    // Flag to check if the player is inside the trigger zone
    private bool playerInRange = false;
    public GameObject interactText;
    public CinemachineVirtualCamera mainCamera;
    private GameObject currentNPC; // Reference to the GameObject this script is attached to
    public speechController speechController;
    private void Start()
    {
        // Assign the current GameObject to currentNPC
        currentNPC = gameObject;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        interactText.SetActive(true);
        Debug.Log("Player entered trigger");
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger zone of NPC.");
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        interactText.SetActive(false);
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited trigger zone of NPC.");
            playerInRange = false;
        }
    }

    void Update()
    {
        // Check if the player is in range and presses the "E" key
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(AdjustCameraWithDelay());
        }
    }

    IEnumerator AdjustCameraWithDelay()
    {
        float initialFOV = mainCamera.m_Lens.FieldOfView;
        float targetFOV = 60f;
        float duration = 0.2f;
        float elapsedTime = 0f;
        mainCamera.Follow = currentNPC.transform;
        yield return new WaitForSeconds(1.0f); // Wait for 2 seconds

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            mainCamera.m_Lens.FieldOfView = Mathf.Lerp(initialFOV, targetFOV, elapsedTime / duration);
            yield return null;
        }

        // Ensure the FOV is exactly the target FOV
        mainCamera.m_Lens.FieldOfView = targetFOV;
        StartCoroutine(speechController.RevealText("Hello World"));
    }
}

