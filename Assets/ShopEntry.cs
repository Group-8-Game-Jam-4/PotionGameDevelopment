using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopEntry : MonoBehaviour
{
    public string sceneToLoad;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Press 'E' to change scene");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Changing scene...");
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
