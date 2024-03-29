using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 1f;

    // Update is called once per frame
    public void LoadNextScene(string levelName)
    {
        StartCoroutine(LoadLevel(levelName));
    }

    public IEnumerator LoadLevel(string levelName)
    {
        //Play animation
        transition.SetTrigger("Start");

        //Wait
        yield return new WaitForSeconds(transitionTime);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //Load scene
        SceneManager.LoadScene(levelName);
    }
}
