using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.GUID;


[System.Serializable]
public class PlayerData
{
    //simple values
    public int points = 0;
    public Dictionary<string, Dictionary<string, ItemClass>> totalInventories = new Dictionary<string, Dictionary<string, ItemClass>>();
    public Dictionary<string, List<string[]>> formattedInventories = new Dictionary<string, List<string[]>>();
    public Dictionary<string, QuestClass> ongoingQuests = new Dictionary<string, QuestClass>();


    public PlayerData (Player player)
    {
        points = player.points;
        totalInventories = player.totalInventories;
        formattedInventories = player.formattedInventories;
        ongoingQuests = player.ongoingQuests;
    }
}
