using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class GameplayManager : MonoBehaviour
{
    private Dictionary<string, List<string[]>> questLines = new Dictionary<string, List<string[]>>();
    private Dictionary<string, QuestClass> ongoingQuests = new Dictionary<string, QuestClass>();
    private List<string> npcNames = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        LoadCSV();

        // string name = "Barry";
        // if (questLines.ContainsKey(name))
        // {
        //     List<string[]> values = questLines[name];
        //     if (values != null && values.Count > 0)
        //     {
        //         foreach (string[] array in values)
        //         {
        //             foreach (string str in array)
        //             {
        //                 Debug.Log(str);
        //             }
        //         }
        //     }
        //     else
        //     {
        //         Debug.Log($"No values found for the name {name}");
        //     }
        // }
        // else
        // {
        //     Debug.Log($"No values found for the name {name}");
        // }
    }

    public void GetQuest(string NPCName)
    {
        if(ongoingQuests.ContainsKey(NPCName))
        {
            // gets info from the questClass of that npc

            int textCounter = ongoingQuests[NPCName].TextCounter;
            bool awaitingItems = ongoingQuests[NPCName].AwaitingItems;
            List<string[]> submittedItems = ongoingQuests[NPCName].SubmittedItems;
            List<string[]> neededItems = ongoingQuests[NPCName].NeededItems;

            // get the questlines for that npc
            List<string[]> values = questLines[name];

            // if awaiting items (also check we have questlines left just for saftey)
            if(awaitingItems && textCounter <= values.Count)
            {
                Debug.Log("This npc needs items!");
                // display whatever text is this npcs like i need items text. At the start of the csv for that npc there will be 2 values one for awaiting items one for no quests rn
            }
            else
            {
                // check to see if it has any questlines left
                if(textCounter <= values.Count)
                {
                    // yipppeeee time to load a questline
                    string[] array = values.ElementAt(textCounter);
                    foreach (string str in array)
                    {
                        Debug.Log("CURRENT QUESTLINE");
                        Debug.Log(str);
                    }

                    // here we need to iterate the textCounter by 1 so were on the next bit. Obviously this is different if awaiting items as we need to stay on that one
                    textCounter += 1;
                    ongoingQuests[NPCName].TextCounter += 1;
                }
                else
                {
                    // display no remaining quests text
                    Debug.Log("There are no remaining quests for this npc!");
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
            List<string[]> values = questLines[name];

            // do the first bit of text
            string[] array = values.ElementAt(0);
            foreach (string str in array)
            {
                Debug.Log("CURRENT QUESTLINE YOU HAVENT INTERACTED WITH THIS GUY BEFORE");
                Debug.Log(str);
            }

            // here we need to iterate the textCounter by 1 so were on the next bit
            ongoingQuests[NPCName].TextCounter += 1;
        }

    }

    void LoadCSV()
    {
        // imports the csv
        TextAsset textFile = Resources.Load<TextAsset>("questLines");

        Debug.Log(textFile);

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
}
