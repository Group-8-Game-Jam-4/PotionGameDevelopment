using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class furnaceController : MonoBehaviour
{
    public int furnaceLvl;
    public Image furnaceImage;
    public Sprite[] furnaceSprites; // Array to hold UI images
    public WorkstationSystem workStationScript;
    private int clicksNeeded;
    public float timeIntervalInSeconds = 3f; // Adjust this value as needed
    private WaitForSeconds waitTime;

    void Start()
    {
        // Set initial clicks needed
        SetClicksNeeded();
        UpdateFurnaceSprite();

        waitTime = new WaitForSeconds(timeIntervalInSeconds);
        StartCoroutine(DecrementFurnaceLevel());
    }



    public void Update()
    {
        if (furnaceLvl == 4)
        {
            workStationScript.BrewPotion();
            furnaceLvl = 0;
        }

        furnaceImage.sprite = furnaceSprites[furnaceLvl];
    }

    IEnumerator DecrementFurnaceLevel()
    {
        while (true)
        {
            yield return waitTime;

            if (furnaceLvl > 0)
            {
                furnaceLvl--;
                furnaceImage.sprite = furnaceSprites[furnaceLvl];
            }
        }
    }

    public void progressLvl()
    {
        clicksNeeded--;
        if (clicksNeeded <= 0 && workStationScript.readyToBrew == true)
        {
            // Increment furnace level and reset clicks needed
            furnaceLvl++;
            SetClicksNeeded();
            UpdateFurnaceSprite();
        }
    }
                                                                            
    void SetClicksNeeded()
    {
        // Generate random number between 1 and 3 for the next progression
        clicksNeeded = Random.Range(1, 4);
    }

    void UpdateFurnaceSprite()
    {
        // Make sure furnaceLvl does not exceed the number of sprites
        furnaceLvl = Mathf.Clamp(furnaceLvl, 0, furnaceSprites.Length - 1);

        // Update the sprite to the one corresponding to the furnaceLvl
        furnaceImage.sprite = furnaceSprites[furnaceLvl];
    }
}
