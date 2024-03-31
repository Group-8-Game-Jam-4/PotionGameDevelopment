using Cinemachine;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
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


    // all the ui stuff
    public GameObject missingItemsUi;
    public TextMeshProUGUI[] missingText;
    public GameObject[] missingStrikes;

    public GameObject haveItemsUi;
    public TextMeshProUGUI[] haveText;
    public GameObject[] haveStrikes;

    QuestClass lastQuest;
    Inventory playerInventory;
    
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
        playerInventory = player.GetComponentInChildren<PlayerInventory>().inventory;
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            // Debug.Log("Player entered trigger zone of NPC.");
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(interactText != null)
        {
            interactText.SetActive(false);
            ResetUi();
        }
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
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ResetUi();
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
            mainCamera.Follow = player.transform;

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

                    // show the ui where its like do you want to give these items
                    ResetUi();
                    ShowUI(currentQuest);
                    break;
                case 2:
                    // display the text saying there are no more quests available
                    speechController.RevealText(currentQuest.noQuestsAvailable);
                    break;
            }  
        }
    }

    void ShowUI(QuestClass currentQuest)
    {
        Inventory playerInventory = player.GetComponentInChildren<PlayerInventory>().inventory;
        lastQuest = currentQuest;
        bool hasAllItems = true;
        // we need to see if we have all the items in the inventory or not
        for(int i = 0; i < currentQuest.NeededItems.Count; i++)
        {
            // i will be the current item
            ItemClass item = currentQuest.NeededItems[i];
            string className = item.className;

            // this means we have some of that item
            if(playerInventory.totalInventory.ContainsKey(className))
            {
                if(item.quantity <= playerInventory.totalInventory[className].quantity)
                {
                    // enable the strike for this index as we have enough of it
                    missingText[i].text = item.displayName + " x" + item.quantity.ToString();
                    haveText[i].text = item.displayName + " x" + item.quantity.ToString();
                    missingStrikes[i].SetActive(true);
                    haveStrikes[i].SetActive(true);
                }
                else
                {
                    missingText[i].text = item.displayName + " x" + item.quantity.ToString();
                    haveText[i].text = item.displayName + " x" + item.quantity.ToString();
                    hasAllItems = false;
                }
            }
        }

        if(hasAllItems)
        {
            // show gui for whether or not u wanna give them
            missingItemsUi.SetActive(false);
            haveItemsUi.SetActive(true);
        }
        else
        {
            // show the gui showing the needed items
            haveItemsUi.SetActive(false);
            missingItemsUi.SetActive(true);
        }
    }

    void ResetUi()
    {
        for(int i = 0; i < 2; i++)
        {
            missingText[i].text = "";
            haveText[i].text = "";
            missingStrikes[i].SetActive(false);
            haveStrikes[i].SetActive(false);
            haveItemsUi.SetActive(false);
            missingItemsUi.SetActive(false);
        }
    }

    public void GiveQuestItems()
    {
        // take items from player
        foreach(ItemClass item in lastQuest.NeededItems)
        {
            playerInventory.TakeItem(item.className, item.quantity);
        }

        // make it so this quest is like done
        lastQuest.NeededItems = null;
        lastQuest.AwaitingItems = false;
        haveItemsUi.SetActive(false);
        missingItemsUi.SetActive(false);

        // show the next bit of text
        StartCoroutine(AdjustCameraWithDelay());
    }

    public void No()
    {
        haveItemsUi.SetActive(false);
        missingItemsUi.SetActive(false);
    }
}

