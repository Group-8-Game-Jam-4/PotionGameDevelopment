using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BiomeCollider : MonoBehaviour
{
    bool interactable = false;
    public string scenename;
    public GameObject transitioner;
    public GameObject interactText;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && interactable == true)
        {
            transitioner.GetComponent<LevelLoader>().LoadNextScene(scenename);
            interactText.SetActive(false);
            interactable = false;
            //get rid of interact text so it doesnt mess with the animation
        }
    }

    //allows player interaction with biome trigger if they enter its collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            interactable = true;
            interactText.SetActive(true);
            //show interact text
        }
    }

    //disallows player interaction with biome trigger if they exit its collision
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            interactable = false;
            interactText.SetActive(false);
            //get rid of interact text
        }
    }
}
