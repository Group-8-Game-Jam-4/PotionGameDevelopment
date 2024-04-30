using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public GameObject transitioner;

    public void PlayGame()
    {
        Debug.Log("PLAY GAME!");
        transitioner.GetComponent<LevelLoader>().LoadNextScene("FinalGrasslandsScene");
    }

    public void PlayNewSave()
    {
        Player player = GameObject.Find("GameplayManager").GetComponent<Player>();
        player.ResetSave();
        transitioner.GetComponent<LevelLoader>().LoadNextScene("FinalGrasslandsScene");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT GAME!");
        Application.Quit();
    }

    public void OpenURL()
    {
        Application.OpenURL("https://discord.gg/b5NvVWp");
        Debug.Log("INVITED TO https://discord.gg/b5NvVWp");
    }


    public void OpenWIKI()
    {
        Application.OpenURL("https://en.wikipedia.org/wiki/Baked_beans");
        Debug.Log("OPENED https://en.wikipedia.org/wiki/Baked_beans");
    }

    private void Start() {

    }

}

