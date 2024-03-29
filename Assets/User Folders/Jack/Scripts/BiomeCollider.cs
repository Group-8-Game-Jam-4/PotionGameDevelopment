using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BiomeCollider : MonoBehaviour
{
    bool interactable = false;
    public string scenename;
    public GameObject transitioner;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && interactable == true)
        {
            transitioner.GetComponent<LevelLoader>().LoadNextScene(scenename);
            //SceneManager.LoadScene(scenename);
            interactable = false;
        }
    }

    //allows player interaction with biome trigger if they enter its collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            interactable = true;
            Debug.Log(interactable);
        }
    }

    //disallows player interaction with biome trigger if they exit its collision
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            interactable = false;
            Debug.Log(interactable);
        }
    }
}
