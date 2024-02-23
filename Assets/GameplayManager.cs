using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class GameplayManager : MonoBehaviour
{
    private Dictionary<string, List<string[]>> questLines = new Dictionary<string, List<string[]>>();
    private Dictionary<string, string[]> ongoingQuests = new Dictionary<string, string[]>();
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
