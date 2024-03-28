using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Player : MonoBehaviour
{
    //simple values
    public int points = 0;



    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    { 
        PlayerData data = SaveSystem.LoadPlayer();

        points = data.points;
    }


    private void Start() 
    {
        FirstLoad();
        Sync();
    }


    void Sync()
    {
        LoadPlayer();
        SavePlayer();
    }

    void FirstLoad()
    {
        string path = Application.persistentDataPath + "/player.ezeSave";
        if (File.Exists(path))
        {
            LoadPlayer();
        }
        else
        {
            SavePlayer();
        }
    }
}