using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.GUID;

public class Player : MonoBehaviour
{
    //simple values
    public int points = 0;
    public Dictionary<string, Dictionary<string, ItemClass>> totalInventories = new Dictionary<string, Dictionary<string, ItemClass>>();
    public Dictionary<string, List<string[]>> formattedInventories = new Dictionary<string, List<string[]>>();
    public Dictionary<string, QuestClass> ongoingQuests = new Dictionary<string, QuestClass>();



    public void SaveGame()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadGame()
    { 
        if(SaveSystem.LoadPlayer() != null)
        {
            PlayerData data = SaveSystem.LoadPlayer();

            points = data.points;
            totalInventories = data.totalInventories;
            formattedInventories = data.formattedInventories;
            ongoingQuests = data.ongoingQuests;
        }
        else
        {
            SaveGame();
        }
    }

    public void ResetSave()
    {
        SaveGame();
    }
}