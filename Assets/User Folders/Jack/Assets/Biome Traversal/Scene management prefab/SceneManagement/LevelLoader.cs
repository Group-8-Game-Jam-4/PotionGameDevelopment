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

        //i've commented this out bc i think morgan's trees use the cursor to break so id rather the cursor stay
        //commented out so its not lost in case it gets put back in
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        //Load scene
        SceneManager.LoadScene(levelName);
    }
}
