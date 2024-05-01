using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    private Dictionary<string, List<string[]>> questLines = new Dictionary<string, List<string[]>>();
    private Dictionary<string, QuestClass> ongoingQuests = new Dictionary<string, QuestClass>();
    private List<string> npcNames = new List<string>();
    private PlayerInventory playerInventory;

    // Start is called before the first frame update
    void Start()
    {
        playerInventory = GameObject.Find("Player").GetComponent<PlayerInventory>();
        LoadQuests();
        LoadCSV();
    }

    public QuestClass GetQuest(string NPCName)
    {
        LoadQuests();
        if(ongoingQuests.ContainsKey(NPCName))
        {
            // gets info from the questClass of that npc

            int textCounter = ongoingQuests[NPCName].TextCounter;
            bool awaitingItems = ongoingQuests[NPCName].AwaitingItems;
            List<ItemClass> neededItems = ongoingQuests[NPCName].NeededItems;
            ItemClass rewardItem = ongoingQuests[NPCName].rewardItem;

            // get the questlines for that npc
            List<string[]> values = questLines[NPCName];

            // if awaiting items (also check we have questlines left just for saftey)
            if(awaitingItems && textCounter <= values.Count)
            {
                // Debug.Log("This npc needs items!");
                ongoingQuests[NPCName].state = 1;
                // display whatever text is this npcs like i need items text. At the start of the csv for that npc there will be 2 values one for awaiting items one for no quests rn
            }
            else
            {
                // check to see if it has any questlines left
                if(textCounter <= values.Count)
                {
                    // yipppeeee time to load a questline
                    string[] array = values.ElementAt(textCounter);
                    SplitInfo(array, NPCName);

                    // here we need to iterate the textCounter by 1 so were on the next bit. Obviously this is different if awaiting items as we need to stay on that one
                    textCounter += 1;
                    ongoingQuests[NPCName].TextCounter += 1;
                }
                else
                {
                    // display no remaining quests text
                    // Debug.Log("There are no remaining quests for this npc!");
                    ongoingQuests[NPCName].state = 2;
                }
            }
        }
        else
        {
            // this npc hasnt been interacted with before so time for new stuff yayyy

            // add them to the ongoingQuests
            ongoingQuests.Add(NPCName, new QuestClass{});

            // display their text this is probs something along the lines of hey traveller im barry or something

            // get the questlines for that npc
            List<string[]> values = questLines[NPCName];

            // get the default no quests and needed item text lines
            ongoingQuests[NPCName].noQuestsAvailable = values.ElementAt(0)[1];
            ongoingQuests[NPCName].doYouHaveThis = values.ElementAt(1)[1];

            // do the first bit of text
            string[] array = values.ElementAt(2);
            SplitInfo(array, NPCName);

            // here we need to iterate the textCounter by 1 so were on the next bit
            ongoingQuests[NPCName].TextCounter += 1;
        }

        SaveQuests();
        return ongoingQuests[NPCName];
    }

    void SplitInfo(string[] array, string NPCName)
    {
        string questText = array[1];
        string reward = array[5];


        // assign the current questline to the quest class so the npc can display it
        ongoingQuests[NPCName].currentStoryline = questText;
        ongoingQuests[NPCName].state = 0;

        Debug.Log($"StoryLine: {questText}");


        // loops are slow and theres no point trying to do this every time if were already waiting for items
        if(!ongoingQuests[NPCName].AwaitingItems)
        {
            ongoingQuests[NPCName].NeededItems = new List<ItemClass>();
            for(int i = 2; i < 5; i++)
            {
                if(array[i] != "")
                {
                    Debug.Log($"Needed item: {array[i]}");

                    // fill out the needed items in the quest class (wont do this until the inventory is like existing)
                    ongoingQuests[NPCName].AwaitingItems = true;
                    ongoingQuests[NPCName].state = 1;

                    if(array[i].Contains("*"))
                    {
                        string[] parts = array[i].Split('*');
                        string itemName = parts [0];
                        int quantity;

                        if (int.TryParse(parts[1], out quantity))
                        {
                            // setup the item
                            ItemClass item = new ItemClass
                            {
                                className = itemName,
                                quantity = quantity
                            };

                            // get the items display name
                            if (playerInventory.inventory.totalInventory.ContainsKey(itemName))
                            {
                                item.displayName = playerInventory.inventory.totalInventory[itemName].displayName;
                                ongoingQuests[NPCName].NeededItems.Add(item);
                                Debug.Log($"Added {quantity} of {itemName} to the npcs needed items");
                            }
                            else
                            {
                                Debug.LogError($"Failed to add item: {itemName} to the npc {NPCName}'s needed items due to an invalid item name: '{itemName}'");
                            }
                        }
                        else
                        {
                            Debug.LogError($"Failed to add item: {itemName} to the npc {NPCName}'s needed items due to an invalid quantity of: '{parts[1]}'");
                        }
                    }

                    // fill out the needed items in the quest class
                }
            }
            // set reward
            if(reward != null && reward != "")
            {
                if(reward.Contains("*"))
                {
                    string[] parts = reward.Split('*');
                    string itemName = parts [0];
                    int quantity;

                    if (int.TryParse(parts[1], out quantity))
                    {
                        // setup the item
                        ItemClass item = new ItemClass
                        {
                            className = itemName,
                            quantity = quantity
                        };

                        // get the items display name
                        if (playerInventory.inventory.totalInventory.ContainsKey(itemName))
                        {
                            item.displayName = playerInventory.inventory.totalInventory[itemName].displayName;
                            ongoingQuests[NPCName].rewardItem = item;
                            Debug.Log($"Added {quantity} of {itemName} to the npcs reward");
                        }
                        else
                        {
                            Debug.LogError($"Failed to add item: {itemName} to the npc {NPCName}'s reward due to an invalid item name: '{itemName}'");
                        }
                    }
                    else
                    {
                        Debug.LogError($"Failed to add item: {itemName} to the npc {NPCName}'s reward due to an invalid quantity of: '{parts[1]}'");
                    }
                }
            }
        }
    }

    void LoadCSV()
    {
        // imports the csv
        TextAsset textFile = Resources.Load<TextAsset>("questLines");

        // Debug.Log(textFile);

        // splits into lines
        string[] splittedLines = textFile.text.Split("\n");

        string prevName = "";
        List<string[]> questsList = new List<string[]>();

        // splits into items
        for (int i = 2; i < splittedLines.Length; i++)
        {
            //Debug.Log(splittedLines[i]);
            string[] splittedValues = ParseCSVLine(splittedLines[i]);

            // if it's a new NPC name, add it to the list and create a new quest list
            if (splittedValues[0] != "" && splittedValues[0] != prevName)
            {
                npcNames.Add(splittedValues[0]);

                // Create a new quest list for this NPC
                questsList = new List<string[]>();

                // Add the new quest list to the hashmap by name
                if(questLines.ContainsKey(splittedValues[0])){questLines.Remove(splittedValues[0]);}
                questLines.Add(splittedValues[0], questsList);

                // set the new prevName
                prevName = splittedValues[0];
            }

            // adds this line to the quest list
            questsList.Add(splittedValues);
        }
    }

    public string[] ParseCSVLine(string csvLine)
    {
        List<string> fields = new List<string>();
        bool insideQuotes = false;
        string currentField = "";

        foreach (char c in csvLine)
        {
            if (c == ',' && !insideQuotes)
            {
                fields.Add(currentField);
                currentField = "";
            }
            else if (c == '"')
            {
                insideQuotes = !insideQuotes;
                // Optionally skip adding the quote to the field
            }
            else
            {
                currentField += c;
            }
        }

        fields.Add(currentField); // Add the last field
        return fields.ToArray();
    }

    public void SaveQuests()
    {
        Player saveSystem = GameObject.Find("GameplayManager").GetComponent<Player>();
        saveSystem.ongoingQuests = ongoingQuests;
        saveSystem.SaveGame();
    }

    public void LoadQuests()
    {
        Player saveSystem = GameObject.Find("GameplayManager").GetComponent<Player>();
        saveSystem.LoadGame();
        LoadCSV();
        if(saveSystem.ongoingQuests == null)
        {
            SaveQuests();
        }
        ongoingQuests = saveSystem.ongoingQuests;
    }
}
