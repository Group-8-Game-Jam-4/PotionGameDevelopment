using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    //simple values
    public int points = 0;


    public PlayerData (Player player)
    {
        points = player.points;
    }
}
