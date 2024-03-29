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
    public SpeechController speechController;
    public string NPCname;
    private Transform player;
    float initialFOV;
    bool camAtNPC = false;


    private void Start()
    {
        // Assign the current GameObject to currentNPC
        currentNPC = gameObject;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        interactText.SetActive(true);
        // Debug.Log("Player entered trigger");
        player = other.gameObject.transform;
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            // Debug.Log("Player entered trigger zone of NPC.");
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        interactText.SetActive(false);
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            // Debug.Log("Player exited trigger zone of NPC.");
            playerInRange = false;
            StartCoroutine(ResetCamera());
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

    IEnumerator ResetCamera()
    {
        if(camAtNPC)
        {
            float currentFov = mainCamera.m_Lens.FieldOfView;
            float targetFOV = initialFOV;
            float duration = 0.2f;
            float elapsedTime = 0f;
            mainCamera.Follow = currentNPC.transform;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                mainCamera.m_Lens.FieldOfView = Mathf.Lerp(currentFov, targetFOV, elapsedTime / duration);
                yield return null;
            }

            camAtNPC = false;
            mainCamera.m_Lens.FieldOfView = initialFOV;
        }
    }

    IEnumerator AdjustCameraWithDelay()
    {
        if(!camAtNPC)
        {
            initialFOV = mainCamera.m_Lens.FieldOfView;
            float targetFOV = 60f;
            float duration = 0.2f;
            float elapsedTime = 0f;
            mainCamera.Follow = currentNPC.transform;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                mainCamera.m_Lens.FieldOfView = Mathf.Lerp(initialFOV, targetFOV, elapsedTime / duration);
                yield return null;
            }
            camAtNPC = true;

            // Ensure the FOV is exactly the target FOV
            mainCamera.m_Lens.FieldOfView = targetFOV;
        }

        if(speechController.isRevealing)
        {
            speechController.SkipText();
        }
        else
        {
            // here we will call the gameplay manager which handles the storylines. We will parse in whatever npc this is probably based on the game object name or somth
            GameplayManager gameplayManager = GameObject.FindObjectOfType<GameplayManager>();

            QuestClass currentQuest = gameplayManager.GetQuest(NPCname);
            switch (currentQuest.state)
            {
                case 0:
                    // display the standard storyline like text
                    speechController.RevealText(currentQuest.currentStoryline);
                    break;
                case 1:
                    // display the text asking for the missing items and then probably display like a ui where the player can transfer them
                    speechController.RevealText(currentQuest.doYouHaveThis);
                    break;
                case 2:
                    // display the text saying there are no more quests available
                    speechController.RevealText(currentQuest.noQuestsAvailable);
                    break;
            }  
        }
    }
}

